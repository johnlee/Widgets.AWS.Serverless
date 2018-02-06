using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Widgets.Lambda.Data;

namespace Widgets.Lambda.Controllers
{
    [Route("api/[controller]")]
    public class WidgetsController : Controller
    {
        Repository _repository;

        public WidgetsController(IAmazonDynamoDB client)
        {
            this._repository = new Repository(client);
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var getBatch = await _repository.GetWidgetsByBatchUsingOP();
                var getSingle = await _repository.GetWidgetByIdUsingOP(null);
                var getArbitrary = await _repository.GetWidgetAndArbitraryTypeUsingOP();
                var query = await _repository.GetWidgetsByQueryUsingOP(null);
                var scan = await _repository.GetWidgetsByScanUsingOP();

                return Ok(new
                {
                    GetByBatch = getBatch,
                    GetBySingle = getSingle,
                    WidgetAndType = getArbitrary,
                    Query = query,
                    Scan = scan
                });
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    Error = e.Message
                });
            }
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var widget = await _repository.GetWidgetByIdUsingOP(id);
            if (widget != null)
            {
                return Ok(widget);
            }
            return NotFound();
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Widget widget)
        {
            try
            {
                // Add the widget given
                _repository.AddWidget(widget);

                // Add 3 random objects using different access api
                var lowLevel = await _repository.GetWidgetUsingLowLevel();
                var documentModel = await _repository.GetWidgetUsingDocumentModel();
                var objectModel = await _repository.GetWidgetUsingObjectPersistence();

                return Ok(new
                {
                    Original = widget,
                    LowLevel = lowLevel,
                    DocumentModel = documentModel,
                    ObjectModel = objectModel
                });
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
        
        [HttpPut]
        public IActionResult Put([FromBody]Widget widget)
        {
            try
            {
                _repository.UpdateWidget(widget);
                return Ok("Success");
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
        
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                _repository.DeleteWidget(id);
                return Ok("Success");
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScaffoldedApi.Interfaces;
using ScaffoldedApi.QueryFilter;

namespace ScaffoldedApi
{
    public class ScaffoldedController<T> : ControllerBase where T : class
    {
        protected readonly IScaffoldedDataService<T> _dataService;

        public ScaffoldedController(IScaffoldedDataService<T> dataService)
        {
            _dataService = dataService;
        }

        [HttpGet(""), QueryFilter]
        public virtual async Task<ActionResult<IEnumerable<T>>> GetAll(CancellationToken cancellationToken = default)
        {
            return await InvokeService(svc => 
                svc.GetAllAsync(HttpContext.Request.Query.ToDictionary(x => x.Key, x => (object)x.Value), cancellationToken));
        }

        [HttpGet("{keys}")]
        public virtual async Task<ActionResult<T>> Get(string keys, CancellationToken cancellationToken = default)
        {
            return await InvokeService(svc => svc.GetAsync(keys, cancellationToken));
        }

        [HttpPost("")]
        public virtual async Task<ActionResult<T>> Add([FromBody] T model, CancellationToken cancellationToken = default)
        {
            return await InvokeService(svc => svc.AddAsync(model, cancellationToken));
        }

        [HttpPut("{keys}")]
        public virtual async Task<ActionResult<T>> Update(string keys, [FromBody] T model, CancellationToken cancellationToken = default)
        {
            return await InvokeService(svc => svc.UpdateAsync(keys, model, cancellationToken));
        }

        [HttpDelete("{keys}")]
        public virtual async Task<ActionResult> Delete(string keys, CancellationToken cancellationToken = default)
        {
            return await InvokeService(svc => svc.DeleteAsync(keys, cancellationToken));
        }

        protected virtual async Task<ActionResult<TResult>> InvokeService<TResult>(Func<IScaffoldedDataService<T>, Task<TResult>> method)
        {
            try
            {
                return Ok(await method(_dataService));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        protected virtual async Task<ActionResult> InvokeService(Func<IScaffoldedDataService<T>, Task> method)
        {
            try
            {
                await method(_dataService);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

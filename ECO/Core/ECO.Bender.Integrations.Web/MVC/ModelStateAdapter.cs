using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace ECO.Bender.Web.MVC
{
    public class ModelStateAdapter
    {
        #region Private_fields

        private ModelStateDictionary _ModelState;

        private IList<string> _ResourceBinding = new List<string>();
        
        private OperationResult _Result;

        private bool _Converted = false;

        #endregion

        #region Ctor

        internal ModelStateAdapter(ModelStateDictionary modelState, OperationResult result)
        {
            _ModelState = modelState;
            _Result = result;
        }

        #endregion

        #region Public_Methods

        public ModelStateAdapter AddResourceBinding(string resourceName)
        {
            if (!_Converted)
            {
                _ResourceBinding.Add(resourceName);
                return this;
            }
            else
            {
                throw new ApplicationException("The ModelStateAdapter was already converted");
            }
        }

        public void Convert()
        {
            _Result.Errors.ToList().ForEach(error =>
            {
                string context = error.Context;
                string description = error.Description;
                foreach (string resourceName in _ResourceBinding)
                {
                    string message = System.Convert.ToString(HttpContext.GetGlobalResourceObject(resourceName, description));
                    if (!string.IsNullOrEmpty(message))
                    {
                        description = message;
                    }
                }
                _ModelState.AddModelError(context, description);
            });
            _Converted = true;
        }

        #endregion
    }

    public static class ModelStateExtensions
    {
        public static ModelStateAdapter GetAdapter(this ModelStateDictionary modelState, OperationResult result)
        {
            return new ModelStateAdapter(modelState, result);
        }
    }
}

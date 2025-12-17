using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BC_Control_Models;

namespace BC_Control_System.Service
{
    public class ViewTransitionNavigator
    {
        #region 私有字段
        private readonly IRegionManager _regionManager;
        private readonly ILogOpration _logOpration;
        
        #endregion

        public ViewTransitionNavigator(IRegionManager regionManager,ILogOpration logOpration)
        {
            _regionManager = regionManager;
            _logOpration= logOpration;
        }
        #region 公共方法
        public void MainVeiwNavigation(string viewName)
        {
            try
            {
                _regionManager.Regions["ContentRegion"].RequestNavigate(viewName, GetNavigationRseult);
            }
            catch (Exception ex)
            {

                _logOpration.WriteError(ex.ToString());
            }
           
        }
        public void MainVeiwNavigation(string viewName, NavigationParameters keys)
        {
            try
            {
                
                _regionManager.Regions["ContentRegion"].RequestNavigate(viewName, GetNavigationRseult, keys);
            }
            catch (Exception ex)
            {
                _logOpration.WriteError(ex.ToString());
            }
            
        }
        public void TraceLogViewNavigation(string viewName)
        {
            try
            {
                _regionManager.Regions["TraceLogContentRegion"].RequestNavigate(viewName, GetNavigationRseult);
            }
            catch (Exception ex)
            {
                _logOpration.WriteError(ex.ToString());
            }
        }
        public void NavigationAware(string viewName,string regionName)
        {
            try
            {
                _regionManager.Regions[regionName].RequestNavigate(viewName, GetNavigationRseult);
            }
            catch (Exception ex)
            {
                _logOpration.WriteError(ex.ToString());
            }
        }
        public void RecipeLogViewNavigation(string viewName, NavigationParameters keys)
        {
            try
            {
                _regionManager.Regions["ModuleRecipeContentRegion"].RequestNavigate(viewName, GetNavigationRseult, keys);
            }
            catch (Exception ex)
            {
                _logOpration.WriteError(ex.ToString());
            }
        }
        #endregion
        #region
        private void GetNavigationRseult(NavigationResult navigationResult)
        {
            if (navigationResult.Result==true)
            {
                _logOpration.WriteInfo(navigationResult.Context.ToString());
            }
            else
                _logOpration.WriteError($"{navigationResult.Context} {navigationResult.Error}");
        }
        #endregion

    }
}

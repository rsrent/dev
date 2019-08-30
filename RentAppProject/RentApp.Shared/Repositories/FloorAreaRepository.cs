using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModuleLibraryShared.Services;
using RentAppProject;

namespace RentApp.Shared.Repositories
{
    public class FloorAreaRepository
    {
        private readonly HttpClientProvider _clientProvider;
        private readonly IErrorMessageHandler _errorHandler;

        public FloorAreaRepository(HttpClientProvider clientProvider, IErrorMessageHandler errorHandler)
        {
            _clientProvider = clientProvider;
            _errorHandler = errorHandler;
        }

        public async Task Plans(Action<List<CleaningPlan>> success, Action error = null)
        {
            await _clientProvider.Client.Get("/api/PlanFloorArea/Plans/", successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task Floors(Action<List<Floor>> success, Action error = null)
        {
            await _clientProvider.Client.Get("/api/PlanFloorArea/Floors", successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task Areas(int cleaningPlanID, Action<List<Area>> success, Action error = null)
        {
            await _clientProvider.Client.Get("/api/PlanFloorArea/Areas/" + cleaningPlanID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task AddFloor(Floor floor, Action<Floor> success, Action error = null)
        {
            await _clientProvider.Client.Post("/api/PlanFloorArea/AddFloor", content: floor, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task AddArea(int cleaningPlanID, Area area, Action<Area> success, Action error = null)
        {
            await _clientProvider.Client.Post("/api/PlanFloorArea/AddArea/" + cleaningPlanID, content: area, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }
    }
}

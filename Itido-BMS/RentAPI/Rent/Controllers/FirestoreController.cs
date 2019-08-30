using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rent.Data;
using Rent.Models;
using Rent.Models.TimePlanning;
using Rent.Repositories;

// export GOOGLE_APPLICATION_CREDENTIALS="bms-firestore.json"
// set GOOGLE_APPLICATION_CREDENTIALS=bms-firestore.json

namespace Rent.Controllers
{

    [Produces("application/json")]
    [Route("api/Firestore")]
    public class FirestoreController : ControllerExecutor
    {
        private readonly FirestoreCommunicationRepository _firestoreRepository;
        public FirestoreController(FirestoreCommunicationRepository firestoreRepository)
        {
            _firestoreRepository = firestoreRepository;
        }

        [HttpPost("CreateConversationWithUsers")]
        public async Task<IActionResult> CreateConversationWithUsers([FromBody] ICollection<int> userIds)
        => await Executor(async () => await _firestoreRepository.CreateConversationWithUserIds(Requester, userIds));

        [HttpPost("PostMessage/{conversationId}")]
        public async Task<IActionResult> PostMessage([FromRoute] string conversationId, [FromBody] FirestoreMessage message)
        => await Executor(async () => await _firestoreRepository.PostMessage(Requester, conversationId, message));

        [HttpGet("GetFirestoreConversations")]
        public IActionResult GetFirestoreConversations()
        => Executor(() => _firestoreRepository.GetFirestoreConversations(Requester));


        //[AllowAnonymous]
        //[HttpGet("Update")]
        //public async Task<IActionResult> Update()
        //=> await Executor(async () => await _firestoreRepository.CreateConversationsForLocationsWithout());

    }
}

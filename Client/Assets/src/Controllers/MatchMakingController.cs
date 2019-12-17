using Messages;
using Poster;
using UnityEngine;
using UnityEngine.Events;

namespace Controllers
{
    public class MatchMakingController : MonoBehaviour
    {
        public IMessageBinder Binder = MessageBusStatic.Bus;
        public IMessageSender Sender = MessageBusStatic.Bus;

        public GameState State;

        public UnityEvent OnMatchMaked;

        public void OnEnable()
        {
            Binder.Bind<StartRoomSessionMessage>(StartRoom);
        }
        
        public void OnDisable()
        {
            Binder.UnBind<StartRoomSessionMessage>(StartRoom);
        }

        private void StartRoom(StartRoomSessionMessage obj)
        {
            Debug.Log("Room found!");
            
            State.IsPlayerHasAuthority = obj.IsAuthority;
            State.IsNetworkMatch = true;

            OnMatchMaked.Invoke();
        }

        public void Start()
        {
            Sender.Send(new StartSearchRoomMessage());
        }
    }
}
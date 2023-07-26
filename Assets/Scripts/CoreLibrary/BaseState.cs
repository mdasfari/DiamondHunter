namespace CoreLibrary
{
    public class BaseState<T>
    {
        public string Name { get; set; }

        public T StateID { get; private set; }

        public BaseState(T id)
        {
            StateID = id;
        }

        public BaseState(T id, string name) : this(id)
        {
            Name = Name;
        }

        public BaseState(T id, BaseStateDelegate onEnter, BaseStateDelegate onExit = null, BaseStateDelegate onUpdate = null, BaseStateDelegate onFixedUpdate = null) : this(id)
        {
            OnEnter = onEnter;
            OnExit = onExit;
            OnUpdate = onUpdate;
            OnFixedUpdate = onFixedUpdate;
        } 

        public BaseState(T id, string name, BaseStateDelegate onEnter, BaseStateDelegate onExit = null, BaseStateDelegate onUpdate = null, BaseStateDelegate onFixedUpdate = null) : this(id, name)
        {
            OnEnter = onEnter;
            OnExit = onExit;
            OnUpdate = onUpdate;
            OnFixedUpdate = onFixedUpdate;
        }

        public delegate void BaseStateDelegate();
        public BaseStateDelegate OnEnter;
        public BaseStateDelegate OnExit;
        public BaseStateDelegate OnUpdate;
        public BaseStateDelegate OnFixedUpdate;

        virtual public void Enter() { OnEnter?.Invoke(); }
        virtual public void Exit() { OnExit?.Invoke(); }
        virtual public void Update() { OnUpdate?.Invoke(); }
        virtual public void FixedUpdate() { OnFixedUpdate?.Invoke(); }
    }
}
using UnityEngine;

public abstract class GameSystem : MonoBehaviour
{
    [Tooltip("System hane init order nhỏ hơn sẽ init trước")]
    [SerializeField] private int initOrder = 0;

    public int InitOrder => initOrder;

    public virtual void Init(GameContext _gameContext)
    {

    }

    public virtual void ShutDown()
    {

    }
}

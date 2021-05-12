using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour, IInteraction
{
    #region Animation Transition
    private const int Idle = 0;
    private const int Move = 1;
    private const int Shave = 2;
    private const int FurGrow = 3;
    #endregion

    private readonly Vector3 LookAtLeftVector = new Vector3(1, 1, 1);
    private readonly Vector3 LookAtRightVector = new Vector3(-1, 1, 1);

    [Header("# Sprite Property")]
    [SerializeField] private Sprite _SheepSprite;
    [SerializeField] private Sprite _FurrySprite;
    [SerializeField] private SpriteRenderer _Renderer;
    [SerializeField] private Animator _Animator;
    private int _AnimControlKey;

    [Header("# Fur Property")]
    [SerializeField][Min(1)] private int _ShaveWoolMin;
    [SerializeField][Min(1)] private int _ShaveWoolMax;
    [SerializeField] private float _FurGrowTime;

    [Header("# Movement Property")]
    [SerializeField] private float _MoveSpeed;
    
    [Space()]
    [SerializeField] private float _MovingTimeMin;
    [SerializeField] private float _MovingTimeMax;

    [Space()]
    [SerializeField] private float _WaitingTimeMin;
    [SerializeField] private float _WaitingTimeMax;

    private bool _IsShaved = false;
    private IEnumerator _MovementRoutine;

    private void OnEnable()
    {
        StartCoroutine(MovementPeriod());
        RegisterInteraction();

        _AnimControlKey = _Animator.GetParameter(0).nameHash;
    }
    private void SetNaturalAnimState()
    {
        if (_MovementRoutine != null)
        {
            _Animator.SetInteger(_AnimControlKey, Move);
        }
        else
        {
            _Animator.SetInteger(_AnimControlKey, Idle);
        }
    }
    private IEnumerator MovementPeriod()
    {
        while (gameObject.activeInHierarchy)
        {
            float waiting = Random.Range(_WaitingTimeMin, _WaitingTimeMax);

            for (float i = 0f; i < waiting; i += Time.deltaTime * Time.timeScale)
                yield return null;

            _Animator.SetInteger(_AnimControlKey, Move);
            yield return StartCoroutine(_MovementRoutine = MovementRoutine());
        }
    }
    private IEnumerator MovementRoutine()
    {
        float moving = Random.Range(_MovingTimeMin, _MovingTimeMax);
        Vector3 movingDir;

        if (Random.value > 0.5f)
        {
            movingDir = Vector3.left;
            transform.localScale = LookAtLeftVector;
        }
        else
        {
            movingDir = Vector3.right;
            transform.localScale = LookAtRightVector;
        }
        float deltaTime = Time.deltaTime * Time.timeScale;

        for (float i = 0f; i < moving; i += deltaTime)
        {
            transform.position += movingDir * deltaTime * _MoveSpeed;
            yield return null;

            deltaTime = Time.deltaTime * Time.timeScale;
        }
        _MovementRoutine = null;
        _Animator.SetInteger(_AnimControlKey, Idle);
    }
    private IEnumerator FurGrowRoutine()
    {
        for (float i = 0f; i < _FurGrowTime; i += Time.deltaTime * Time.timeScale)
            yield return null;

        _Animator.SetInteger(_AnimControlKey, FurGrow);
    }

    // ========== IInteraction ========== //
    public GameObject InteractObject()
    {
        return gameObject;
    }
    public void OperateAction<T>(T xValue) where T : IItemFunction
    {
        if (_IsShaved) return;

        _Animator.SetInteger(_AnimControlKey, Shave);
    }
    public void RegisterInteraction()
    {
        Player_Interaction.Instance.InObjRegister(gameObject.GetInstanceID(), this);
    }
    // ========== IInteraction ========== //

    private void AE_Shave()
    {
        int dropItemCount = Random.Range(_ShaveWoolMin, _ShaveWoolMax);
        for (int i = 0; i < dropItemCount; i++)
        {
            ItemExisting item = ItemMaster.Instance.TakeItemExisting(ItemList.WOOL);

            item.transform.position = transform.position;
            item.gameObject.SetActive(true);
        }
        _Renderer.sprite = _SheepSprite;
        _IsShaved = true;

        StartCoroutine(FurGrowRoutine());
        SetNaturalAnimState();
    }
    private void AE_FurGrow()
    {
        _IsShaved = false;
        _Renderer.sprite = _FurrySprite;

        SetNaturalAnimState();
    }
}
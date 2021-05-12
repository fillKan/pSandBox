using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour, IInteraction
{
    private readonly Vector3 LookAtLeftVector = new Vector3(1, 1, 1);
    private readonly Vector3 LookAtRightVector = new Vector3(-1, 1, 1);

    [Header("# Sprite Property")]
    [SerializeField] private Sprite _SheepSprite;
    [SerializeField] private Sprite _FurrySprite;
    [SerializeField] private SpriteRenderer _Renderer;

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

    private void OnEnable()
    {
        StartCoroutine(MovementPeriod());
        RegisterInteraction();
    }
    private IEnumerator MovementPeriod()
    {
        while (gameObject.activeInHierarchy)
        {
            float waiting = Random.Range(_WaitingTimeMin, _WaitingTimeMax);

            for (float i = 0f; i < waiting; i += Time.deltaTime * Time.timeScale)
                yield return null;

            yield return StartCoroutine(MovementRoutine());
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
    }
    private IEnumerator FurGrowRoutine()
    {
        for (float i = 0f; i < _FurGrowTime; i += Time.deltaTime * Time.timeScale)
            yield return null;

        _IsShaved = false;
        _Renderer.sprite = _FurrySprite;
    }

    // ========== IInteraction ========== //
    public GameObject InteractObject()
    {
        return gameObject;
    }
    public void OperateAction<T>(T xValue) where T : IItemFunction
    {
        if (_IsShaved) return;

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
    }
    public void RegisterInteraction()
    {
        Player_Interaction.Instance.InObjRegister(gameObject.GetInstanceID(), this);
    }
    // ========== IInteraction ========== //
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absorb : MonoBehaviour, IAbility
{
    string AbilityName = "Absorb";
    [SerializeField] float continualTime = 0.5f; // ����ʱ�䣬0��ʾ˲��
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // �ڲ��߼�
    IEnumerator WaitAndDo(float seconds, System.Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }

    // ʵ�ֽӿ�IAbility
    public void Activate(IEntity speller)
    {
        if (speller == null)
        {
            Debug.LogError("speller is null");
            return;
        }
        var triggeringObj = speller.TriggeringObject();
        if (triggeringObj == null)
        {
            Debug.LogError("speller.TriggeringObject() is null");
            return;
        }
        var entity = triggeringObj.GetComponent<IEntity>();
        if (entity == null)
        {
            Debug.LogError("triggeringObj.GetComponent<IEntity>() is null");
            return;
        }
        if (entity.GetGameObject() == null)
        {
            Debug.LogError("entity.GetGameObject() is null");
            return;
        }
        if (!entity.GetGameObject().activeInHierarchy)
        {
            Debug.LogError("entity.GetGameObject() is not active in hierarchy");
            return;
        }
        if (entity != null && entity.GetGameObject().activeInHierarchy && entity.IsDead())
        {
            speller.SetCertainStatus("isAbsorbing", true);
            StartCoroutine(WaitAndDo(continualTime, () =>
            {
                var abilities = entity.GetAbilities();
                // �����ռ���
                foreach (var ability in abilities)
                {
                    speller.AddAbility(ability);
                }
                // ������Ч��
                var effects = entity.GetEffects();
                foreach (var effect in effects)
                {
                    effect.GetComponent<IEffect>()?.ApplyEffect(speller);
                }
                speller.SetCertainStatus("isAbsorbing", false);
                Destroy(entity.GetGameObject());
            }));
        }
    }
    public bool IsContinual()
    {
        return continualTime > 0;
    }
    public IEffect EffectBeforeExecute()
    {
        //return new EffectBeforeAbsorb();
        return null;
    }
    public string GetAbilityName()
    {
        return AbilityName;
    }
}

// �ڲ���ʵ��IEffect
class EffectBeforeAbsorb : IEffect
{
    public void ApplyEffect(IEntity entity)
    {
        entity.SetCertainStatus("isAbsorbing", true);
    }

    public IEntity GetCreator()
    {
        // �ɸ�����Ҫ����null�������߼�
        return null;
    }
}

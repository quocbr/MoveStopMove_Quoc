using System;
using UnityEngine;



public class Weapon : GameUnit
{
    public const float TIME_WEAPON_RELOAD = 0.5f;
    [SerializeField] GameObject child;
    [SerializeField] private MeshRenderer meshRenderer;

    public bool IsCanAttack => child.activeSelf;

    private Material material;

    Bullet bullet;

    public Material Material { get => material;}

    private void Start()
    {
        material = meshRenderer.material;
    }

    private void SetActive(bool active)
    {
        child.SetActive(active);
    }

    private void OnEnable()
    {
        SetActive(true);
    }

    public void Throw(Character character, Action<Character, Character> onHit)
    {
        
        switch (poolType)
        {
            case PoolType.Axe:
                bullet = HBPool.Spawn<Axe>(PoolType.B_Axe, character.ThrowPos.transform.position, Quaternion.identity);
                break;
            case PoolType.Axe_1:
                bullet = HBPool.Spawn<Axe>(PoolType.B_Axe_1, character.ThrowPos.transform.position, Quaternion.identity);
                break;
            case PoolType.Knife:
                bullet = HBPool.Spawn<Knife>(PoolType.B_Knife, character.ThrowPos.transform.position, Quaternion.identity);
                break;
            case PoolType.Boomerang:
                bullet = HBPool.Spawn<Boomerang>(PoolType.B_Boomerang, character.ThrowPos.transform.position, Quaternion.identity);
                break;
            case PoolType.Hammer:
                bullet = HBPool.Spawn<Hammer>(PoolType.B_Hammer, character.ThrowPos.transform.position, Quaternion.identity);
                break;
            case PoolType.Candy_0:
                bullet = HBPool.Spawn<Candy>(PoolType.B_Candy_0, character.ThrowPos.transform.position, Quaternion.identity);
                break;
            case PoolType.Candy_2:
                bullet = HBPool.Spawn<Candy>(PoolType.B_Candy_2, character.ThrowPos.transform.position, Quaternion.identity);
                break;
            case PoolType.Candy_1:
                bullet = HBPool.Spawn<Candy>(PoolType.B_Candy_1, character.ThrowPos.transform.position, Quaternion.identity);
                break;
            case PoolType.Candy_4:
                bullet = HBPool.Spawn<Candy>(PoolType.B_Candy_4, character.ThrowPos.transform.position, Quaternion.identity);
                break;
            
        }
        SetActive(false);
        //Bullet bullet = HBPool.Spawn<Bullet>(PoolType.Bullet,character.throwPos.transform.position,Quaternion.identity);
        bullet.OnInit(Material,character, onHit);

        Invoke(nameof(OnEnable), TIME_WEAPON_RELOAD);
    }

    //public void HideMesh(bool hide)
    //{
    //    meshRenderer.enabled = hide;
    //}

    public void ChangeMaterial(Material material)
    {
        this.material = material;
        Material[] materials = meshRenderer.materials;
        for (int i = 0;i<this.meshRenderer.materials.Length;i++)
        {
            materials[i] = material;
        }
        meshRenderer.materials = materials;
    }
}

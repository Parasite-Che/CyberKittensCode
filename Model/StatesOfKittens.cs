using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatesOfKittens
{
    public abstract void CallingTheFirstKitten(KittenStateLogic kittenStateLogic);
    
    public abstract void CallingTheSecondKitten(KittenStateLogic kittenStateLogic);
    
    public abstract void CallingTheThirdKitten(KittenStateLogic kittenStateLogic);

}

class FirstKitten : StatesOfKittens
{
    public override void CallingTheFirstKitten(KittenStateLogic kittenStateLogic)
    {
        Debug.LogError("First Kitten was called!!!");
    }

    public override void CallingTheSecondKitten(KittenStateLogic kittenStateLogic)
    {
        kittenStateLogic.StatesOfKittens = new SecondKitten();
        kittenStateLogic.LightRifle.SetActive(false);
        kittenStateLogic.HeavyRifle.SetActive(true);
        kittenStateLogic.WeaponLogic = kittenStateLogic.HeavyRifle.GetComponent<Rifle>();
        kittenStateLogic.HeavyRifle.transform.parent.GetComponent<PlayerController>().weaponTransform =
            kittenStateLogic.HeavyRifle.transform;
        
        kittenStateLogic.HeavyRifle.GetComponent<Rifle>().reloadProgress = 100;
        kittenStateLogic.ResetFromChangeKitten();
    }

    public override void CallingTheThirdKitten(KittenStateLogic kittenStateLogic)
    {
        kittenStateLogic.StatesOfKittens = new ThirdKitten();
        kittenStateLogic.LightRifle.SetActive(false);
        kittenStateLogic.Claws.SetActive(true);
        kittenStateLogic.WeaponLogic = kittenStateLogic.Claws.GetComponent<Katana>();
        kittenStateLogic.Claws.transform.parent.GetComponent<PlayerController>().weaponTransform =
            kittenStateLogic.Claws.transform;
        
        kittenStateLogic.Claws.GetComponent<Katana>().reloadProgress = 100;
        kittenStateLogic.ResetFromChangeKitten();
    }
    
}

class SecondKitten : StatesOfKittens
{
    public override void CallingTheFirstKitten(KittenStateLogic kittenStateLogic)
    {
        kittenStateLogic.StatesOfKittens = new FirstKitten();
        kittenStateLogic.HeavyRifle.SetActive(false);
        kittenStateLogic.LightRifle.SetActive(true);
        kittenStateLogic.WeaponLogic = kittenStateLogic.LightRifle.GetComponent<Rifle>();
        kittenStateLogic.LightRifle.transform.parent.GetComponent<PlayerController>().weaponTransform =
            kittenStateLogic.LightRifle.transform;
        
        kittenStateLogic.LightRifle.GetComponent<Rifle>().reloadProgress = 100;
        kittenStateLogic.ResetFromChangeKitten();
    }

    public override void CallingTheSecondKitten(KittenStateLogic kittenStateLogic)
    {
        Debug.LogError("Second Kitten was called!!!");
    }

    public override void CallingTheThirdKitten(KittenStateLogic kittenStateLogic)
    {
        kittenStateLogic.StatesOfKittens = new ThirdKitten();
        kittenStateLogic.HeavyRifle.SetActive(false);
        kittenStateLogic.Claws.SetActive(true);
        kittenStateLogic.WeaponLogic = kittenStateLogic.Claws.GetComponent<Katana>();
        kittenStateLogic.Claws.transform.parent.GetComponent<PlayerController>().weaponTransform =
            kittenStateLogic.Claws.transform;
        
        kittenStateLogic.Claws.GetComponent<Katana>().reloadProgress = 100;
        kittenStateLogic.ResetFromChangeKitten();
    }
    
}

class ThirdKitten : StatesOfKittens
{
    public override void CallingTheFirstKitten(KittenStateLogic kittenStateLogic)
    {
        kittenStateLogic.StatesOfKittens = new FirstKitten();
        kittenStateLogic.Claws.SetActive(false);
        kittenStateLogic.LightRifle.SetActive(true);
        kittenStateLogic.WeaponLogic = kittenStateLogic.LightRifle.GetComponent<Rifle>();
        kittenStateLogic.LightRifle.transform.parent.GetComponent<PlayerController>().weaponTransform =
            kittenStateLogic.LightRifle.transform;
        
        kittenStateLogic.LightRifle.GetComponent<Rifle>().reloadProgress = 100;
        kittenStateLogic.ResetFromChangeKitten();
    }

    public override void CallingTheSecondKitten(KittenStateLogic kittenStateLogic)
    {
        kittenStateLogic.StatesOfKittens = new SecondKitten();
        kittenStateLogic.Claws.SetActive(false);
        kittenStateLogic.HeavyRifle.SetActive(true);
        kittenStateLogic.WeaponLogic = kittenStateLogic.HeavyRifle.GetComponent<Rifle>();
        kittenStateLogic.HeavyRifle.transform.parent.GetComponent<PlayerController>().weaponTransform =
            kittenStateLogic.HeavyRifle.transform;
        
        kittenStateLogic.HeavyRifle.GetComponent<Rifle>().reloadProgress = 100;
        kittenStateLogic.ResetFromChangeKitten();
    }

    public override void CallingTheThirdKitten(KittenStateLogic kittenStateLogic)
    {
        Debug.LogError("Third Kitten was called!!!");
    }

}

public class KittenStateLogic
{
    public GameObject Claws;
    public GameObject LightRifle;
    public GameObject HeavyRifle;
    
    public StatesOfKittens StatesOfKittens { get; set; }
    public IWeapon WeaponLogic;
    
    public void GunsInitializing(GameObject claws, GameObject lightRifle, GameObject heavyRifle)
    {
        Claws = claws;
        LightRifle = lightRifle;
        HeavyRifle = heavyRifle;

        Claws.GetComponent<IWeapon>().Initialize(Claws.transform.parent.tag);
        LightRifle.GetComponent<IWeapon>().Initialize(Claws.transform.parent.tag);
        HeavyRifle.GetComponent<IWeapon>().Initialize(Claws.transform.parent.tag);
        
        WeaponLogic = LightRifle.GetComponent<Rifle>();
        LightRifle.transform.parent.GetComponent<PlayerController>().weaponTransform = LightRifle.transform;
    }

    public void ResetFromChangeKitten()
    {
        Objects.instance.Field.SetActive(false);
    }
    
}

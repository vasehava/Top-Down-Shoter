using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RewardSystem
{
    public int waveNumber;
    public RewardWeapons rewardWep;
}

public enum RewardWeapons
{
    AK12 = 0,
    M4MB = 1
}
public class WaveRewardSystem : MonoBehaviour {
    public List<RewardSystem> rewards = new List<RewardSystem>();
    public GameObject prefabAK12, prefabM4MB;
    public void CheckRewards(int wave, int zombiesCount, Vector3 pos)
    {
        if (zombiesCount != 0)
            return;
        pos.y += 1.2f;
        for(int i = 0; i < rewards.Count; i++)
            if(rewards[i].waveNumber == wave)
            {
                switch(rewards[i].rewardWep)
                {
                    case RewardWeapons.AK12:
                        prefabAK12.transform.position = pos;
                        prefabAK12.SetActive(true);
                        break;
                    case RewardWeapons.M4MB:
                        prefabM4MB.transform.position = pos;
                        prefabM4MB.SetActive(true);
                        break;
                    default: break;
                }
                return;
            }
    }

}

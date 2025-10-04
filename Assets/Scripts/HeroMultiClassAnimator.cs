using System.Collections.Generic;
using UnityEngine;

public class HeroMultiClassAnimator : MonoBehaviour
{
    [System.Serializable]
    public class MultiClassAnimationRule
    {
        public List<HeroType> requiredTypes;
        public RuntimeAnimatorController animatorController;
    }

    [SerializeField] private List<MultiClassAnimationRule> multiClassRules;

    /// <summary>
    /// Returns a multiclass animation if there is any for current classes list
    /// </summary>
    public RuntimeAnimatorController UpdateMultiClassAnimator(RuntimeAnimatorController anim, List<HeroType> currentTypes)
    {
        if (currentTypes.Count <= 1)
            return anim;

        foreach (var rule in multiClassRules)
        {
            if (MatchesRule(currentTypes, rule.requiredTypes))
            {
                return rule.animatorController;
            }
        }
        return anim;
    }

    /// <summary>
    /// Shows if the given classes suits required classes for an animation
    /// </summary>
    /// <param name="currentTypes"></param>
    /// <param name="requiredTypes"></param>
    /// <returns></returns>
    private bool MatchesRule(List<HeroType> currentTypes, List<HeroType> requiredTypes)
    {
        foreach (HeroType requiredType in requiredTypes)
        {
            if (!currentTypes.Contains(requiredType))
                return false;
        }
        return true;
    }
}

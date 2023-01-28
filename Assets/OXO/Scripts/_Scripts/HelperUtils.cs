using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dreamteck.Splines;
using UnityEngine;

namespace MuhammetInce.HelperUtils
{
    public static class HelperUtils
    {
        public static IEnumerator AnimationToggle(Animator animator ,int id, float duration)
        {
            animator.SetBool(id, true);
            yield return new WaitForSeconds(duration);
            animator.SetBool(id, false);
        }

        public static void SpeedUp(SplineFollower follower)
        {
            follower.followSpeed = GameController.Instance.craneSpeed;
        }
        public static void StopIt(SplineFollower follower)
        {
            follower.followSpeed = 0.000001f;
        }

        public static void ListFucker<T>(this IEnumerable<T> numerable)
        {

            for (int i = 0; i < numerable.Count(); i++)
            {
                if (numerable.ElementAt(i) as GameObject)
                {
                    object mono = numerable.ElementAt(i);

                    GameController.Instance.DestroyObj(mono);
                }
            }

            List<T> list = numerable as List<T>;
            list.Clear();
        }
    }
}


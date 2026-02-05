using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Block.Animations
{
    public class AnimatorComponent : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public void SetAnimatorController(AnimatorOverrideController animatorOverride)
        {
            _animator.runtimeAnimatorController = animatorOverride;
            if(_animator.runtimeAnimatorController != null)
                _animator.speed = 0;
        }

        public async UniTaskVoid PlayIdleAnimation(float delayStart)
        {
            if (delayStart > 0)
            {
                await UniTask.Delay(System.TimeSpan.FromSeconds(delayStart));
            }
            
            _animator.speed = 1;
            if (_animator.runtimeAnimatorController != null)
                _animator.Play(nameof(AnimationState.Idle));
        }

        public UniTask PlayDestroyAnimation(CancellationToken token)
        {
            _animator.speed = 1;
            
            _animator.Play(nameof(AnimationState.Destroy));
            return UniTask.WaitUntil(() =>
            {
                var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
                return stateInfo.IsName(nameof(AnimationState.Destroy)) && stateInfo.normalizedTime >= 0.95f;
            }, cancellationToken: token);
        }
    }

    public enum AnimationState
    {
        Idle,
        Destroy,
    }
}
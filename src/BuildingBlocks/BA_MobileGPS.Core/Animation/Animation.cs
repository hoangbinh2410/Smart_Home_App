using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BA_MobileGPS.Core
{
    public enum AnimationType
    {
        Scale,
        Opacity,
        TranslationX,
        TranslationY,
        Rotation
    }

    public class Animation
    {
        private readonly Dictionary<string, ViewTransition[]> _stateTransitions = new Dictionary<string, ViewTransition[]>();

        public void Add(object state, ViewTransition[] viewTransitions)
        {
            var stateStr = state?.ToString().ToUpperInvariant();

            if (string.IsNullOrEmpty(stateStr) || viewTransitions == null)
                throw new NullReferenceException("Value of 'state', 'viewTransitions' cannot be null");

            if (_stateTransitions.ContainsKey(stateStr))
                throw new ArgumentException($"State {state} already added");

            _stateTransitions.Add(stateStr, viewTransitions);
        }

        public async Task Go(object newState, bool withAnimation = true)
        {
            var newStateStr = newState?.ToString().ToUpperInvariant();

            if (string.IsNullOrEmpty(newStateStr))
                throw new NullReferenceException("Value of newState cannot be null");

            if (!_stateTransitions.ContainsKey(newStateStr))
                throw new KeyNotFoundException($"There is no state {newState}");

            // Get all ViewTransitions
            var viewTransitions = _stateTransitions[newStateStr];

            // Get transition tasks
            var tasks = viewTransitions.Select(viewTransition => viewTransition.GetTransition(withAnimation));

            // Run all transition tasks
            await Task.WhenAll(tasks);
        }
    }

    public class ViewTransition
    {
        private readonly AnimationType _animationType;
        private readonly int _delay;
        private readonly double _length;
        private readonly Easing _easing;
        private readonly double _endValue;
        private readonly WeakReference<VisualElement> _targetElementReference;

        public ViewTransition(VisualElement targetElement, AnimationType animationType, double endValue, double length = 250, Easing easing = null, int delay = 0)
        {
            _targetElementReference = new WeakReference<VisualElement>(targetElement);
            _animationType = animationType;
            _length = length;
            _endValue = endValue;
            _delay = delay;
            _easing = easing;
        }

        public async Task GetTransition(bool withAnimation)
        {
            if (!_targetElementReference.TryGetTarget(out VisualElement targetElement))
                throw new ObjectDisposedException("Target VisualElement was disposed");

            if (_delay > 0)
                await Task.Delay(_delay);

            withAnimation &= _length > 0;

            switch (_animationType)
            {
                case AnimationType.Scale:
                    if (withAnimation)
                        await targetElement.ScaleTo(_endValue, (uint)_length, _easing);
                    else
                        targetElement.Scale = _endValue;
                    break;

                case AnimationType.Opacity:
                    if (withAnimation)
                    {
                        if (!targetElement.IsVisible && _endValue <= 0)
                            break;

                        if (targetElement.IsVisible && _endValue < targetElement.Opacity)
                        {
                            await targetElement.FadeTo(_endValue, (uint)_length, _easing);
                            targetElement.IsVisible = _endValue > 0;
                        }
                        else if (targetElement.IsVisible && _endValue > targetElement.Opacity)
                        {
                            await targetElement.FadeTo(_endValue, (uint)_length, _easing);
                        }
                        else if (!targetElement.IsVisible && _endValue > targetElement.Opacity)
                        {
                            targetElement.Opacity = 0;
                            targetElement.IsVisible = true;
                            await targetElement.FadeTo(_endValue, (uint)_length, _easing);
                        }
                    }
                    else
                    {
                        targetElement.Opacity = _endValue;
                        targetElement.IsVisible = _endValue > 0;
                    }
                    break;

                case AnimationType.TranslationX:
                    if (withAnimation)
                        await targetElement.TranslateTo(_endValue, targetElement.TranslationY, (uint)_length, _easing);
                    else
                        targetElement.TranslationX = _endValue;
                    break;

                case AnimationType.TranslationY:
                    if (withAnimation)
                        await targetElement.TranslateTo(targetElement.TranslationX, _endValue, (uint)_length, _easing);
                    else
                        targetElement.TranslationY = _endValue;
                    break;

                case AnimationType.Rotation:
                    if (withAnimation)
                        await targetElement.RotateTo(_endValue, (uint)_length, _easing);
                    else
                        targetElement.Rotation = _endValue;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
using System.Collections.Generic;

public class Observable<T>
{
    public delegate void ValueChangedEventHandler(T value);
    public event ValueChangedEventHandler ValueChanged;

    public delegate void ValuChangedFromPreviousEventHandler(T previousValue, T newValue);
    public event ValuChangedFromPreviousEventHandler ValueChangedFromPrevious;

    public Observable()
    {
        _value = default;
    }

    public Observable(T initialValue)
    {
        _value = initialValue;
    }

    private T _value;
    public T Value
    {
        get { return _value; }
        set
        {
            T originalValue = _value;
            _value = value;

            if (EqualityComparer<T>.Default.Equals(originalValue, value) == false)
            {
                ValueChanged?.Invoke(value);
                ValueChangedFromPrevious?.Invoke(originalValue, value);
            }
        }
    }

    public static implicit operator T(Observable<T> observable)
    {
        return observable.Value;
    }
}

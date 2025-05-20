using System;

public class SimpleList<T>
{
    private T[] _array = new T[1]; //T타입의 배열 선언
    private int _lastIdx = 0; //배열을 다룰 때 사용할 인덱스값

    public int Count => _lastIdx; //Count 속성으로 _lastIdx를 반환

    public T this[int index] //인덱서
    {
        get
        {
            if (index < 0 || index >= _lastIdx)
                throw new Exception(message: $"[SimpleList]인덱스의 범위를 벗어났습니다.");
            //인덱스가 0보다 작거나 _lastIdx보다 크면 예외 발생
            return _array[index];
        }
        set
        {
            if (index < 0 || index >= _lastIdx)
                throw new Exception(message: $"[SimpleList]인덱스의 범위를 벗어났습니다.");
            _array[index] = value;
        }
    }

    /// <summary>
    /// 리스트에 새로운 요소를 추가 
    /// </summary>
    /// <param name="value">리스트에 추가할 요소</param>
    public void Add(T value)
    {
        // 배열의 크기가 Count보다 작거나 같으면
        // 크기가 2배인 새로운 배열을 생성
        // _array에 할당

        // 배열에 value를 저장

        if(_array.Length <= Count)
        {
            T[] newArray = new T[Count * 2];
            Array.Copy(sourceArray: _array, destinationArray: newArray, length: Count);
            // 복사할 배열: _array,붙여넣을  배열: newArray, 복사할 길이: Count

            _array = newArray;
        }

        _array[_lastIdx] = value; //배열에 value를 저장
        _lastIdx++; //마지막 인덱스 증가
    }

    /// <summary>
    /// 리스트에서 요소를 제거
    /// </summary>
    /// <param name="valueToRemove"></param>
    public void Remove(T value)
    {
        //해당요소가 리스트에 없으면 바로 종료
        //있으면 요소를 제거하고 뒷칸부터 앞으로 당겨옴
        //마지막 인덱스 -1

        if(!TryGetValue(value, out int index))
        {
            return; //리스트에 요소가 없으면 종료
        }
        //리스트에 요소가 있으면
        //해당 요소를 제거하고 뒷칸부터 앞으로 당겨옴

        for (int i = index; i < _lastIdx - 1; i++)
        {
            _array[i] = _array[i + 1]; //앞으로 당겨옴
        }
        _array[_lastIdx - 1] = default; //마지막 인덱스에 default값 저장
        _lastIdx--; //마지막 인덱스 -1



    }

    /// <summary>
    /// 리스트에 요소가 있는지 확인 후 반환
    /// </summary>
    /// <param name="value">포함되어있는지 찾고자 하는 값</param>
    /// <returns></returns>
    public bool Contains(T value)
    {
        //리스트에 요소가 있는지 확인
        //있으면 true, 없으면 false 반환
        return TryGetValue(value, out int _dex);
    }



    /// <summary>
    /// 리스트에서 요소를 찾고 인덱스를 반환
    /// </summary>
    /// <param name="valueout">찾으려는 요소</param>
    /// <param name="index">반환되는 인덱스값</param>
    /// <returns></returns>
    bool TryGetValue(T valueout, out int index)
    {
        //리스트에서 요소를 찾고 인덱스를 반환
        //찾지 못하면 -1을 반환
        index = -1;

        for (int i = 0; i < Count; i++)
        {
            T savedValue = _array[i];

            //배열에 있는 요소가 찾으려는 요소와 같으면
            if (savedValue.Equals(valueout))
            {
                index = i;
                return true; //찾으면 true 반환
            }

        }
        return false;
    }


}

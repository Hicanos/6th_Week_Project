using System;

public class SimpleList<T>
{
    private T[] _array = new T[1]; //TŸ���� �迭 ����
    private int _lastIdx = 0; //�迭�� �ٷ� �� ����� �ε�����

    public int Count => _lastIdx; //Count �Ӽ����� _lastIdx�� ��ȯ

    public T this[int index] //�ε���
    {
        get
        {
            if (index < 0 || index >= _lastIdx)
                throw new Exception(message: $"[SimpleList]�ε����� ������ ������ϴ�.");
            //�ε����� 0���� �۰ų� _lastIdx���� ũ�� ���� �߻�
            return _array[index];
        }
        set
        {
            if (index < 0 || index >= _lastIdx)
                throw new Exception(message: $"[SimpleList]�ε����� ������ ������ϴ�.");
            _array[index] = value;
        }
    }

    /// <summary>
    /// ����Ʈ�� ���ο� ��Ҹ� �߰� 
    /// </summary>
    /// <param name="value">����Ʈ�� �߰��� ���</param>
    public void Add(T value)
    {
        // �迭�� ũ�Ⱑ Count���� �۰ų� ������
        // ũ�Ⱑ 2���� ���ο� �迭�� ����
        // _array�� �Ҵ�

        // �迭�� value�� ����

        if(_array.Length <= Count)
        {
            T[] newArray = new T[Count * 2];
            Array.Copy(sourceArray: _array, destinationArray: newArray, length: Count);
            // ������ �迭: _array,�ٿ�����  �迭: newArray, ������ ����: Count

            _array = newArray;
        }

        _array[_lastIdx] = value; //�迭�� value�� ����
        _lastIdx++; //������ �ε��� ����
    }

    /// <summary>
    /// ����Ʈ���� ��Ҹ� ����
    /// </summary>
    /// <param name="valueToRemove"></param>
    public void Remove(T value)
    {
        //�ش��Ұ� ����Ʈ�� ������ �ٷ� ����
        //������ ��Ҹ� �����ϰ� ��ĭ���� ������ ��ܿ�
        //������ �ε��� -1

        if(!TryGetValue(value, out int index))
        {
            return; //����Ʈ�� ��Ұ� ������ ����
        }
        //����Ʈ�� ��Ұ� ������
        //�ش� ��Ҹ� �����ϰ� ��ĭ���� ������ ��ܿ�

        for (int i = index; i < _lastIdx - 1; i++)
        {
            _array[i] = _array[i + 1]; //������ ��ܿ�
        }
        _array[_lastIdx - 1] = default; //������ �ε����� default�� ����
        _lastIdx--; //������ �ε��� -1



    }

    /// <summary>
    /// ����Ʈ�� ��Ұ� �ִ��� Ȯ�� �� ��ȯ
    /// </summary>
    /// <param name="value">���ԵǾ��ִ��� ã���� �ϴ� ��</param>
    /// <returns></returns>
    public bool Contains(T value)
    {
        //����Ʈ�� ��Ұ� �ִ��� Ȯ��
        //������ true, ������ false ��ȯ
        return TryGetValue(value, out int _dex);
    }



    /// <summary>
    /// ����Ʈ���� ��Ҹ� ã�� �ε����� ��ȯ
    /// </summary>
    /// <param name="valueout">ã������ ���</param>
    /// <param name="index">��ȯ�Ǵ� �ε�����</param>
    /// <returns></returns>
    bool TryGetValue(T valueout, out int index)
    {
        //����Ʈ���� ��Ҹ� ã�� �ε����� ��ȯ
        //ã�� ���ϸ� -1�� ��ȯ
        index = -1;

        for (int i = 0; i < Count; i++)
        {
            T savedValue = _array[i];

            //�迭�� �ִ� ��Ұ� ã������ ��ҿ� ������
            if (savedValue.Equals(valueout))
            {
                index = i;
                return true; //ã���� true ��ȯ
            }

        }
        return false;
    }


}

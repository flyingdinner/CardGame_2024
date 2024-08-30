using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public enum CardType
    {
        none,
        active,// ����� ����������� ���� ���    
        pasive,// �� ������� �����
        hot,   // �� �����/�� ������ ���� �������
    }

    public class CardBase : MonoBehaviour
    {
        [field: SerializeField] public CardType types { get; private set; }
        
        
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Cards
{    

    public class CardShopPanel : MonoBehaviour
    {
        [field: SerializeField] public CardHolder cardHolder;

        [SerializeField] private CardCollectionSO _collectionSO;
        [SerializeField] private HolderPoint[] _shopCardPoints;
        [SerializeField] private HolderPoint[] _playerCardPoints;
        [SerializeField] private GameObject _shopGo;
        [SerializeField] private GameObject _buttonClose;

        [Header("UI Elements")]
        [SerializeField] private GameObject _info_cardLimit;
        [SerializeField] private Button[] _buttonsBy;
        
        [Header("-- debug Serialize --")]
        [SerializeField] private PlayerHuman _player;
        [SerializeField] private List<CardBase> cardsToSinglSpawn;

        void Start()
        {
            _shopGo.SetActive(false);
            _buttonClose.SetActive(false);
            _info_cardLimit.SetActive(false);

            cardsToSinglSpawn = _collectionSO.collection.ToList();
        }

        public void Show(PlayerHuman player)
        {
            _player = player;
            _shopGo.SetActive(true);

            for (int i = 0; i < cardHolder.cardPositions.Length; i++)
            {
                if (cardHolder.cardPositions[i].card == null) continue;

                CardBase card = cardHolder.cardPositions[i].card;

                _playerCardPoints[i].card = card;

                card.transform.parent = _playerCardPoints[i].point;
                card.transform.localScale = Vector3.one;
                card.transform.localPosition = Vector3.zero;
                card.transform.localRotation = Quaternion.identity;
            }

            InitShopButtons();
            _info_cardLimit.SetActive(cardHolder.cardsInHend.Count >= _playerCardPoints.Length);
        }

        private void InitShopButtons()
        {
            TryCreatRandomCardInShopPool();

            bool maxCardsBlock = !HasFreePosition();

            _info_cardLimit.SetActive(maxCardsBlock);

            for (int i = 0; i < _shopCardPoints.Length; i++)
            {
                _buttonsBy[i].gameObject.SetActive(_shopCardPoints[i].card!=null && !maxCardsBlock);
            }

            _buttonClose.SetActive(true);
        }

        private bool HasFreePosition()
        {
            bool hasFree = false;

            for (int i = 0; i < _playerCardPoints.Length; i++)
            {
                if (_playerCardPoints[i].card==null)
                {
                    _playerCardPoints[i].buttonRemove.SetActive(false);
                    hasFree = true;
                }
                else
                {
                    _playerCardPoints[i].buttonRemove.SetActive(true);
                }
            }

            return hasFree;
        }


        public void OnButtonTryBy(int id)
        {
            CardBase card = _shopCardPoints[id].card;

            if (card != null && HasFreePosition())
            {
                if (_player.TryToUseEnergy(card.price))
                {
                    _shopCardPoints[id].card = null;
                    AddCardToPlayerHend(card);
                }
            }
        }

        public void OnButtonTryRemove(int id)
        {
            CardBase card = _playerCardPoints[id].card;

            if (card != null)
            {
                card.transform.parent = null;
                card.transform.position = Vector3.up * 100;
                _playerCardPoints[id].card = null;
                InitShopButtons();
            }
        }

        private void AddCardToPlayerHend(CardBase card)
        {
            HideAllButtons();

            for (int i = 0; i < _playerCardPoints.Length; i++)
            {
                if(_playerCardPoints[i].card == null)
                {
                    _playerCardPoints[i].card = card;

                    card.transform.parent = _playerCardPoints[i].point;
                    card.transform.localScale = Vector3.one;
                    card.transform.localPosition = Vector3.zero;
                    card.transform.localRotation = Quaternion.identity;

                    break;
                }
            }

            InitShopButtons();
        }

        private void HideAllButtons()
        {
            _buttonClose.SetActive(false);
            foreach(Button b in _buttonsBy)
            {
                b.gameObject.SetActive(false);
            }
        }

        private void TryCreatRandomCardInShopPool()
        {            
            foreach(HolderPoint hp in _shopCardPoints)
            {
                if (hp.card != null) continue;

                CardBase card = GetRandomCardFromCollection();

                if (card == null) {
                    Debug.Log(" cards in shop done ");
                    break;
                }

                hp.card = card;
                card.transform.parent = hp.point;
                card.transform.localPosition = Vector3.zero;
                card.transform.localRotation = Quaternion.identity;
                card.transform.localScale = Vector3.one;
            }
        }

        public CardBase GetRandomCardFromCollection()
        {
            if (cardsToSinglSpawn.Count > 0)
            {
                int random = UnityEngine.Random.Range(0, cardsToSinglSpawn.Count);

                CardBase card = cardsToSinglSpawn[random];
                cardsToSinglSpawn.Remove(card);

                CardBase cardGO = Instantiate(card);
                return cardGO;
            }

            return null;

        }

        public void Hide()
        {
            List<CardBase> cards = new List<CardBase>();

            for (int i = 0; i < _playerCardPoints.Length; i++)
            {
                if (_playerCardPoints[i].card == null) continue;
                cards.Add(_playerCardPoints[i].card);
            }

            cardHolder.OnShopClosed(cards);
            _shopGo.SetActive(false);
            _buttonClose.SetActive(false);
        }

        public void ButtonShopClose()//from button
        {
            Hide();
            _player.OnShopCloseButton();
        }

    }
}
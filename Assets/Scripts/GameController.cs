using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameObject fieldPrefab;
        [SerializeField] private GameObject completedScreen;
        [SerializeField] private GameObject gameScreen;
        [SerializeField] private GameObject mainScreen;

        private Player _player;
        private Bot _bot;
        private bool _isEntityMakeStep;
        private Field _field;
        private FieldPositionController _fieldPositionController;
        private FieldSettings _fieldSettings;
        private Coroutine _playCoroutine;

        private void Awake()
        {
            _player = GetComponent<Player>();
            _bot = GetComponent<Bot>();
            
            _field = Instantiate(fieldPrefab).GetComponent<Field>();
            _field.Initialize();
            _field.SetState(false);
            _field.Refresh();
            _field.OnCompleted += OnCompleted;

            _fieldSettings = _field.gameObject.GetComponent<FieldSettings>();
            _fieldPositionController = _field.gameObject.GetComponent<FieldPositionController>();
            _fieldPositionController.SetActive(true);
            
            mainScreen.SetActive(true);
            gameScreen.SetActive(false);
            completedScreen.SetActive(false);
        }

        private void OnCompleted(CellType type)
        {
            StopCoroutine(_playCoroutine);
            _field.SetState(false);
            completedScreen.SetActive(true);
            gameScreen.SetActive(false);
        }

        public void Play()
        {
            _fieldPositionController.SetActive(false);
            _field.SetState(true);
            
            mainScreen.SetActive(false);
            gameScreen.SetActive(true);

            _playCoroutine = StartCoroutine(GameCoroutine());
        }

        private IEnumerator GameCoroutine()
        {
            Entity currentEntity;

            while (true)
            {
                currentEntity = _player;
                currentEntity.OnStep += OnEntityStep;
                _isEntityMakeStep = false;
                currentEntity.GetStep();
                yield return new WaitWhile(() => _isEntityMakeStep == false);
                currentEntity.OnStep -= OnEntityStep;

                currentEntity = _bot;
                currentEntity.OnStep += OnEntityStep;
                _isEntityMakeStep = false;
                currentEntity.GetStep(_field.FieldToArray());
                yield return new WaitUntil(() => _isEntityMakeStep);
                currentEntity.OnStep -= OnEntityStep;
            }
        }

        public void Reset()
        {
            _field.Refresh();
            _field.SetState(false);
            _fieldPositionController.SetActive(true);
            
            mainScreen.SetActive(true);
            gameScreen.SetActive(false);
            completedScreen.SetActive(false);
        }

        private void OnEntityStep(int index, CellType cellType)
        {
            _field.SetSell(index, cellType);
            _isEntityMakeStep = true;
        }

        public void ChangeFieldScale(float value)
        {
            _fieldSettings.ChangeScale(value);
        }

        public void ChangeFieldRotation(float value)
        {
            _fieldSettings.ChangeRotation(value);
        }
    }
}
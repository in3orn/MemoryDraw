using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using Dev.Krk.MemoryDraw.Game.Level;
using Dev.Krk.MemoryDraw.Game.Animations;
using Dev.Krk.MemoryDraw.Data;
using Dev.Krk.MemoryDraw.Inputs;

//TODO refactor - to much responsibilities
namespace Dev.Krk.MemoryDraw.Game
{
    public class LevelController : MonoBehaviour
    {
        public UnityAction OnPlayerFailed;
        public UnityAction<Vector2> OnPlayerMoved;

        public UnityAction OnLevelStarted;
        public UnityAction OnLevelEnded;

        public UnityAction OnLevelCompleted;
        public UnityAction OnLevelFailed;

        public UnityAction OnFlowCompleted;


        public enum StateEnum
        {
            Idle = 0,
            Showing,
            Playing,
            Finished,
            Failed
        }

        private StateEnum state;

        [SerializeField]
        private MapDataProvider levelProvider;

        [SerializeField]
        private FieldMap fieldMap;

        [SerializeField]
        private Player player;

        [SerializeField]
        private Finishable finish;

        [SerializeField]
        private GameObject center;

        [SerializeField]
        private float finishDuration = 1f;

        [SerializeField]
        private LevelAnimator levelAnimator;

        [SerializeField]
        private GameplayInput gameplayInput;


        private Vector2 playerActualPosition;

        private Queue<Vector2> queuedMoves;

        private Queue<Field> queuedFields;
        

        private List<Field> oldHorizontalFields;

        private List<Field> oldVerticalFields;


        private Vector2 offset;


        public int HorizontalLength
        {
            get { return fieldMap.HorizontalLength; }
        }

        public int VerticalLength
        {
            get { return fieldMap.VerticalLength; }
        }


        public StateEnum State { get { return state; } }


        void Awake()
        {
            queuedMoves = new Queue<Vector2>(5);
            queuedFields = new Queue<Field>(5);

            oldHorizontalFields = new List<Field>();
            oldVerticalFields = new List<Field>();
        }


        void OnEnable()
        {
            gameplayInput.OnMoveUpActionLaunched += ProcessMoveUpActionLaunched;
            gameplayInput.OnMoveDownActionLaunched += ProcessMoveDownActionLaunched;
            gameplayInput.OnMoveLeftActionLaunched += ProcessMoveLeftActionLaunched;
            gameplayInput.OnMoveRightActionLaunched += ProcessMoveRightActionLaunched;

            player.OnMoved += ProcessNextMove;

            fieldMap.OnShown += StartGame;

            finish.OnFinished += FinishLevel;
        }

        void OnDisable()
        {
            if(gameplayInput != null)
            {
                gameplayInput.OnMoveUpActionLaunched -= ProcessMoveUpActionLaunched;
                gameplayInput.OnMoveDownActionLaunched -= ProcessMoveDownActionLaunched;
                gameplayInput.OnMoveLeftActionLaunched -= ProcessMoveLeftActionLaunched;
                gameplayInput.OnMoveRightActionLaunched -= ProcessMoveRightActionLaunched;
            }

            if (player != null)
            {
                player.OnMoved -= ProcessNextMove;
            }

            if (fieldMap != null)
            {
                fieldMap.OnShown -= StartGame;
            }

            if (finish != null)
            {
                finish.OnFinished -= FinishLevel;
            }
        }

        private void ProcessMoveUpActionLaunched()
        {
            MoveUp();
        }

        private void ProcessMoveDownActionLaunched()
        {
            MoveDown();
        }

        private void ProcessMoveLeftActionLaunched()
        {
            MoveLeft();
        }

        private void ProcessMoveRightActionLaunched()
        {
            MoveRight();
        }


        public void Init(MapData mapData)
        {
            offset = new Vector2(mapData.Offset.X, mapData.Offset.Y);
            fieldMap.Init(levelProvider.GetMapData(mapData), offset);

            Vector2 playerPosition = (offset + new Vector2(mapData.Start.X, mapData.Start.Y)) * Field.SIZE;
            player.Init(playerPosition, fieldMap.ShowInterval, fieldMap.HideInterval);
            playerActualPosition = player.transform.position;

            Vector2 finishPosition = (offset + new Vector2(mapData.Finish.X, mapData.Finish.Y)) * Field.SIZE;
            finish.Init(finishPosition, fieldMap.ShowInterval, fieldMap.HideInterval);

            queuedFields.Clear();
            queuedMoves.Clear();

            InitCenter();

            state = StateEnum.Idle;
            fieldMap.ShowPreview();
            ShowActors();

            if (OnLevelStarted != null) OnLevelStarted();
        }

        public void Reset()
        {
            ResetOldFields();
        }

        private void ResetOldFields()
        {
            foreach (Field field in oldHorizontalFields)
            {
                Destroy(field.gameObject);
            }
            oldHorizontalFields.Clear();

            foreach (Field field in oldVerticalFields)
            {
                Destroy(field.gameObject);
            }
            oldVerticalFields.Clear();
        }

        public void Clear()
        {
            fieldMap.Clear();
        }

        private void MoveToOld(Field[,] fields, List<Field> oldFields)
        {
            for (int y = 0; y < fields.GetLength(0); y++)
            {
                for (int x = 0; x < fields.GetLength(1); x++)
                {
                    Field field = fields[y, x];
                    if (field != null)
                    {
                        if (field.Valid)
                        {
                            oldFields.Add(field);
                        }
                        else
                        {
                            Destroy(field.gameObject, 2f);
                        }
                    }
                }
            }
        }

        private void InitCenter()
        {
            center.transform.position = (player.transform.position + finish.transform.position) * 0.5f;
        }

        public bool CanMoveLeft()
        {
            return CanMove() && CanMoveLeft(player);
        }

        public bool CanMoveRight()
        {
            return CanMove() && CanMoveRight(player);
        }

        public bool CanMoveUp()
        {
            return CanMove() && CanMoveUp(player);
        }

        public bool CanMoveDown()
        {
            return CanMove() && CanMoveDown(player);
        }

        public bool CanMove()
        {
            return state == StateEnum.Showing || state == StateEnum.Playing;
        }

        private bool CanMoveLeft(Player player)
        {
            Vector2 position = GetFieldPosition(playerActualPosition) + Vector2.left;
            return fieldMap.CanMoveLeft(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        private bool CanMoveRight(Player player)
        {
            Vector2 position = GetFieldPosition(playerActualPosition);
            return fieldMap.CanMoveRight(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        private bool CanMoveUp(Player player)
        {
            Vector2 position = GetFieldPosition(playerActualPosition);
            return fieldMap.CanMoveUp(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        private bool CanMoveDown(Player player)
        {
            Vector2 position = GetFieldPosition(playerActualPosition) + Vector2.down;
            return fieldMap.CanMoveDown(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        public void MoveLeft()
        {
            if (CanMoveLeft())
            {
                Move(getLeftField(player), Vector2.left);
            }
        }

        public void MoveRight()
        {
            if (CanMoveRight())
            {
                Move(getRightField(player), Vector2.right);
            }
        }

        public void MoveUp()
        {
            if (CanMoveUp())
            {
                Move(getUpField(player), Vector2.up);
            }
        }

        public void MoveDown()
        {
            if (CanMoveDown())
            {
                Move(getDownField(player), Vector2.down);
            }
        }

        private void Move(Field field, Vector2 vector)
        {
            playerActualPosition += vector * Field.SIZE;

            if (queuedFields.Count == 0 && player.CanMove())
            {
                PerformMove(field, vector);
            }
            else
            {
                queuedFields.Enqueue(field);
                queuedMoves.Enqueue(vector);
            }
        }

        private void PerformMove(Field field, Vector2 vector)
        {
            if ((state == StateEnum.Showing || state == StateEnum.Playing))
            {
                if (field.Valid)
                {
                    field.Visit(player.transform.position);
                    player.Move(vector * Field.SIZE);

                    if (state == StateEnum.Showing)
                    {
                        state = StateEnum.Playing;
                        fieldMap.ShowPlayMode();
                    }

                    if (OnPlayerMoved != null) OnPlayerMoved(vector);
                }
                else
                {
                    field.Break();
                    playerActualPosition = player.transform.position;
                    queuedFields.Clear();
                    queuedMoves.Clear();
                    if (OnPlayerFailed != null) OnPlayerFailed();
                }
            }
        }

        private void ProcessNextMove()
        {
            if (queuedMoves.Count > 0)
            {
                PerformMove(queuedFields.Dequeue(), queuedMoves.Dequeue());
            }
        }

        private Field getLeftField(Player player)
        {
            Vector2 position = GetFieldPosition(playerActualPosition) + Vector2.left;
            return fieldMap.GetHorizontalField(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        private Field getRightField(Player player)
        {
            Vector2 position = GetFieldPosition(playerActualPosition);
            return fieldMap.GetHorizontalField(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        public Field getUpField(Player player)
        {
            Vector2 position = GetFieldPosition(playerActualPosition);
            return fieldMap.GetVerticalField(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        private Field getDownField(Player player)
        {
            Vector2 position = GetFieldPosition(playerActualPosition) + Vector2.down;
            return fieldMap.GetVerticalField(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }

        private Vector2 GetFieldPosition(Vector2 position)
        {
            return position / Field.SIZE - offset;
        }

        private void StartGame()
        {
            if (state == StateEnum.Idle || state == StateEnum.Failed)
            {
                state = StateEnum.Showing;
            }
        }

        public void FinishLevel()
        {
            state = StateEnum.Finished;

            MoveToOld(fieldMap.HorizontalFields, oldHorizontalFields);
            MoveToOld(fieldMap.VerticalFields, oldVerticalFields);

            fieldMap.HideNotValid();
            HideActors();

            if (OnLevelCompleted != null) OnLevelCompleted();
        }

        public void FailLevel()
        {
            state = StateEnum.Failed;

            MoveToOld(fieldMap.HorizontalFields, oldHorizontalFields);
            MoveToOld(fieldMap.VerticalFields, oldVerticalFields);
            
            int size = fieldMap.HorizontalLength + fieldMap.VerticalLength + (int)offset.x + (int)offset.y + 1; //TODO calculate max
            levelAnimator.FailLevel(oldHorizontalFields, oldVerticalFields, size);

            fieldMap.HideNotValid();
            HideActors();

            center.transform.position = Vector3.zero;

            if (OnLevelFailed != null)
                OnLevelFailed();
        }

        public void CompleteFlow()
        {
            int size = fieldMap.HorizontalLength + fieldMap.VerticalLength + (int)offset.x + (int)offset.y + 1; //TODO calculate max
            levelAnimator.CompleteFlow(oldHorizontalFields, oldVerticalFields, size);

            center.transform.position = Vector3.zero;

            if (OnFlowCompleted != null)
                OnFlowCompleted();
        }

        private void ShowActors()
        {
            player.Show();
            finish.Show();
        }

        private void HideActors()
        {
            player.Hide();
            finish.Hide();
        }
    }
}
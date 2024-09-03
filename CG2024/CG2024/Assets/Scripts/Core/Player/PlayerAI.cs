using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Cards
{
    public class PlayerAI : PlayerBase
    {


        protected override IEnumerator IE_StepProcess_Action(StepBase step, Action callback)
        {
            PlayerBase nearestHumanPlayer = PlayerService.instanse.GetNearestHumanPlayer(Position());

            float distance = Vector3.Distance(nearestHumanPlayer.Position(), Position());

            while (distance > attackRange)
            {
                Vector3 moveDir = PlayerService.instanse.GetNearestPointToMove(Position(), nearestHumanPlayer.Position());
                if (TryToMoveToPoint(moveDir))
                {

                    yield return new WaitForSeconds(0.6f);
                }
                else
                {
                    break;
                }

                distance = Vector3.Distance(nearestHumanPlayer.Position(), Position());
            }

            if(distance <= attackRange && CheckActionDice())
            {
                TryUseDice(DiceValue.action);
                StartCoroutine(IE_Attack(nearestHumanPlayer));
            }

            // End animation
            yield return new WaitForSeconds(1f);
            callback.Invoke();
        }

        protected IEnumerator IeMoveTo(Vector3 point)
        {
            Vector3 startPosition = transform.position;
            Vector3 targetPoint = point;
            float timeElapsed = 0f;
            float duration = 0.4f;

            while (timeElapsed < duration)
            {
                transform.position = Vector3.Lerp(startPosition, targetPoint, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
                yield return null; // чекаємо наступного кадру
            }

            // У кінці гарантовано встановлюємо цільову позицію
            transform.position = targetPoint;
            yield return new WaitForSeconds(0.1f);

            if (!CheckActionDice() && !CheckDice_Move())
            {
                ActionCompleted();
            }
            else
            {
                ReInitTurn();
            }
        }

        private bool TryToMoveToPoint(Vector3 point)
        {
            if (Vector3.Distance(point, Position()) < 0.5)
                return false;

            if (TryUseDice(DiceValue.move))
            {
                StartCoroutine(IeMoveTo(point));
                return true;
            }

            if (TryUseDice(DiceValue.action))
            {
                StartCoroutine(IeMoveTo(point));
                return true;
            }

            return false;
        }

        protected override void ReInitTurn()
        {

        }

        protected override void ActionCompleted()
        {           
            
            
        }
    }
}
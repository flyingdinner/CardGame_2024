using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;  // Камера или объект, за которым нужно следовать
    public float smoothSpeed = 0.125f;  // Скорость следования
    public Vector3 offset;  // Смещение относительно камеры

    void LateUpdate()
    {
        // Целевая позиция с учетом смещения
        Vector3 desiredPosition = target.position + offset;

        // Плавное перемещение к целевой позиции
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Установка позиции объекта
        transform.position = smoothedPosition;

        // Опционально: если нужно, чтобы объект всегда смотрел на камеру
        // transform.LookAt(target);
    }
}

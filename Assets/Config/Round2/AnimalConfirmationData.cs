using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimalConfirmationData", menuName = "Quiz/AnimalConfirmationData")]
public class AnimalConfirmationData : ScriptableObject
{
    [Header("������ ��� ������� ������")]
    [Tooltip("����������� ��������� ��� �������������")]
    public Sprite animalImage;

    [Tooltip("�������� ��������� ��� ������������� �� ���� (��������, '�� ���� ���')")]
    public string animalOnPhotoName;

    [Tooltip("���������� ����� �� ������ '�� ���� *�������� ���������*'?")]
    public bool isCorrectAnimal;

    public string[] answerOptions;  // �������� ������� (����� 4)
    public string correctAnswer;    // ���������� �����
}


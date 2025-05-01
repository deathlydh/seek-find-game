using UnityEngine;
using Unity.Barracuda;

public class YoloDetector : MonoBehaviour
{
    public NNModel modelAsset;
    public Texture2D inputImage; // Сюда перетащишь фото

    private IWorker worker;

    void Start()
    {
        // Загружаем модель и создаем "воркер"
        var model = ModelLoader.Load(modelAsset);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.CSharpBurst, model);

        // Начинаем детекцию
        RunDetection();
    }

    void RunDetection()
    {
        // Конвертируем картинку в тензор
        Tensor input = new Tensor(inputImage, 3);

        // Запускаем инференс (прогон через сеть)
        worker.Execute(input);

        // Получаем выход
        Tensor output = worker.PeekOutput();

        // Здесь нужно расшифровать результат (будет позже)

        input.Dispose();
        output.Dispose();
    }

    void OnDestroy()
    {
        // Чистим память
        worker.Dispose();
    }
}

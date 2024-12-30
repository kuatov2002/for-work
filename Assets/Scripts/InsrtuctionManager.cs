using UnityEngine;
using UnityEngine.UI;

public class InstructionManager : MonoBehaviour
{
    public Image[] instructionSlides;  // Массив изображений для инструкций
    public Button nextButton;          // Кнопка "Далее"
    public Button previousButton;      // Кнопка "Назад"
    public Button closeButton;         // Кнопка "Закрыть"

    private int currentSlideIndex = 0;

    void Start()
    {
        Time.timeScale = 0f;
        // Показываем первый слайд
        ShowSlide(0);

        // Добавляем обработчики событий для кнопок
        nextButton.onClick.AddListener(NextSlide);
        previousButton.onClick.AddListener(PreviousSlide);
        closeButton.onClick.AddListener(CloseInstructions);

        // Скрываем кнопку "Назад" на первом слайде
        previousButton.gameObject.SetActive(false);
    }

    void ShowSlide(int index)
    {
        // Скрываем все слайды
        foreach (var slide in instructionSlides)
        {
            slide.gameObject.SetActive(false);
        }

        // Показываем текущий слайд
        instructionSlides[index].gameObject.SetActive(true);

        // Обновляем видимость кнопок
        previousButton.gameObject.SetActive(index > 0);
        nextButton.gameObject.SetActive(index < instructionSlides.Length - 1);
        closeButton.gameObject.SetActive(index == instructionSlides.Length - 1);
    }

    void NextSlide()
    {
        if (currentSlideIndex < instructionSlides.Length - 1)
        {
            currentSlideIndex++;
            ShowSlide(currentSlideIndex);
        }
    }

    void PreviousSlide()
    {
        if (currentSlideIndex > 0)
        {
            currentSlideIndex--;
            ShowSlide(currentSlideIndex);
        }
    }

    void CloseInstructions()
    {
        Time.timeScale = 1f;
        // Здесь вы можете добавить код для закрытия инструкций
        // Например, деактивировать родительский объект или перейти к началу игры
        gameObject.SetActive(false);
    }
}
import re
import codecs

def decode_unicode_escapes(text):
    """
    Декодирует Unicode-последовательности в строке, корректно обрабатывая escape-последовательности.
    Использует codecs.decode() с 'unicode_escape' для правильной интерпретации.
    """
    return codecs.decode(text, 'unicode_escape')

def process_file(input_filename, output_filename):
    """
    Читает файл, декодирует Unicode-последовательности и записывает результат в новый файл.
    """
    try:
        with open(input_filename, 'r', encoding='utf-8') as infile:
            content = infile.read()

        decoded_content = decode_unicode_escapes(content)

        with open(output_filename, 'w', encoding='utf-8') as outfile:
            outfile.write(decoded_content)

        print(f"Файл успешно обработан. Результат сохранен в {output_filename}")

    except FileNotFoundError:
        print(f"Ошибка: Файл '{input_filename}' не найден.")
    except Exception as e:
        print(f"Произошла ошибка при обработке файла: {e}")


if __name__ == "__main__":
    input_file = input("Введите имя входного файла: ")
    output_file = input("Введите имя выходного файла: ")
    process_file(input_file, output_file)

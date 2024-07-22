using FluentValidation.TestHelper;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Common.Validators.Books;

namespace Library.Test.Validators
{
    public class BookDtoValidatorsTests
    {
        [Theory]
        [MemberData(nameof(GetValidBooksForCreation))]
        public void ValidBookForCreation_ShouldNotHaveValidationError(BookForCreationDto book)
        {
            var creationValidator = new BookForCreationDtoValidator();

            var result = creationValidator.TestValidate(book);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [MemberData(nameof(GetInvalidBooksForCreation))]
        public void InvalidBookForCreation_ShouldHaveValidationError(BookForCreationDto book,
            string errorField)
        {
            var creationValidator = new BookForCreationDtoValidator();

            var result = creationValidator.TestValidate(book);

            result.ShouldHaveValidationErrorFor(errorField);
        }

        [Theory]
        [MemberData(nameof(GetValidBooksForUpdate))]
        public void ValidBookForUpdate_ShouldNotHaveValidationError(BookForUpdateDto book)
        {
            var creationValidator = new BookForUpdateDtoValidator();

            var result = creationValidator.TestValidate(book);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [MemberData(nameof(GetInvalidBooksForUpdate))]
        public void InvalidBookForUpdate_ShouldHaveValidationError(BookForUpdateDto book,
            string errorField)
        {
            var creationValidator = new BookForUpdateDtoValidator();

            var result = creationValidator.TestValidate(book);

            result.ShouldHaveValidationErrorFor(errorField);
        }

        #region TestData

        public static IEnumerable<object[]> GetValidBooksForCreation()
        {
            return
            [
                [
                    new BookForCreationDto
                    {
                        ISBN = "ISBN 9780596520686",
                        Title = "Test book",
                        Genre = "Test genre",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "Test description",
                        ImageName = "testimage.jpeg",
                        Image = new MemoryStream([1,2,1,2,1])
                    }
                ],
                [
                    new BookForCreationDto
                    {
                        ISBN = "9780596520686",
                        Title = "A",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "A",
                        ImageName = "testimage.png",
                        Image = new MemoryStream([1,2,1,2,1])
                    }
                ],
                [
                    new BookForCreationDto
                    {
                        ISBN = "9780596520686",
                        Title = "A",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "A",
                        ImageName = "testimage.png",
                        Image = new MemoryStream([1,2,1,2,1])
                    }
                ]
            ];
        }
        public static IEnumerable<object[]> GetInvalidBooksForCreation()
        {
            return
            [
                [
                    new BookForCreationDto
                    {
                        ISBN = "ISBN 97805965206861",
                        Title = "Test book",
                        Genre = "Test genre",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "Test description",
                        ImageName = "testimage.jpeg",
                        Image = new MemoryStream([1,2,1,2,1])
                    },
                    nameof(BookForCreationDto.ISBN)
                ],
                [
                    new BookForCreationDto
                    {
                        ISBN = "ISBN 978059652068",
                        Title = "Test book",
                        Genre = "Test genre",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "Test description",
                        ImageName = "testimage.jpeg",
                        Image = new MemoryStream([1,2,1,2,1])
                    },
                    nameof(BookForCreationDto.ISBN)
                ],
                [
                    new BookForCreationDto
                    {
                        ISBN = "ISBN",
                        Title = "Test book",
                        Genre = "Test genre",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "Test description",
                        ImageName = "testimage.jpeg",
                        Image = new MemoryStream([1,2,1,2,1])
                    },
                    nameof(BookForCreationDto.ISBN)
                ],
                [
                    new BookForCreationDto
                    {
                        ISBN = "ISBN 97805965206861",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "A",
                        ImageName = "testimage.png",
                        Image = new MemoryStream([1,2,1,2,1])
                    },
                    nameof(BookForCreationDto.Title)
                ],
                [
                    new BookForCreationDto
                    {
                        ISBN = "ISBN 97805965206861",
                        Title = "AbcdefghijklmnopqrstAbcdefghijklmnopqrstu",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "A",
                        ImageName = "testimage.png",
                        Image = new MemoryStream([1,2,1,2,1])
                    },
                    nameof(BookForCreationDto.Title)
                ],
                [
                    new BookForCreationDto
                    {
                        ISBN = "ISBN 97805965206861",
                        Title = "A",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "A",
                        ImageName = "testimage.png",
                        Image = new MemoryStream([1,2,1,2,1])
                    },
                    nameof(BookForCreationDto.Genre)
                ],
                [
                    new BookForCreationDto
                    {
                        ISBN = "ISBN 97805965206861",
                        Title = "A",
                        Genre = "AbcdefghijklmnopqrstAbcdefghijk",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "A",
                        ImageName = "testimage.png",
                        Image = new MemoryStream([1,2,1,2,1])
                    },
                    nameof(BookForCreationDto.Genre)
                ],
                [
                    new BookForCreationDto
                    {
                        ISBN = "ISBN 97805965206861",
                        Title = "A",
                        Genre = "B",
                        Description = "A",
                        ImageName = "testimage.png",
                        Image = new MemoryStream([1,2,1,2,1])
                    },
                    nameof(BookForCreationDto.AuthorId)
                ],
                [
                    new BookForCreationDto
                    {
                        ISBN = "ISBN 97805965206861",
                        Title = "A",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        ImageName = "testimage.png",
                        Image = new MemoryStream([1,2,1,2,1])
                    },
                    nameof(BookForCreationDto.Description)
                ],
                [
                    new BookForCreationDto
                    {
                        ISBN = "ISBN 97805965206861",
                        Title = "A",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "AbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAb",
                        ImageName = "testimage.png",
                        Image = new MemoryStream([1,2,1,2,1])
                    },
                    nameof(BookForCreationDto.Description)
                ],
                [
                    new BookForCreationDto
                    {
                        ISBN = "ISBN 97805965206861",
                        Title = "A",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "C",
                        Image = new MemoryStream([1,2,1,2,1])
                    },
                    nameof(BookForCreationDto.ImageName)
                ],
            ];
        }
        public static IEnumerable<object[]> GetValidBooksForUpdate()
        {
            return
            [
                [
                    new BookForUpdateDto
                    {
                        ISBN = "ISBN 9780596520686",
                        Title = "Test book",
                        Genre = "Test genre",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "Test description",
                        ImageName = "testimage.jpeg",
                        Image = new MemoryStream([1,2,1,2,1])
                    }
                ],
                [
                    new BookForUpdateDto
                    {
                        ISBN = "9780596520686",
                        Title = "A",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "A",
                        ImageName = "testimage.png",
                        Image = new MemoryStream([1,2,1,2,1])
                    }
                ],
                [
                    new BookForUpdateDto
                    {
                        ISBN = "9780596520686",
                        Title = "A",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "A",
                        ImageName = "testimage.png",
                        Image = new MemoryStream([1,2,1,2,1])
                    }
                ]
            ];
        }
        public static IEnumerable<object[]> GetInvalidBooksForUpdate()
        {
            return
            [
                [
                    new BookForUpdateDto
                    {
                        ISBN = "ISBN 97805965206861",
                        Title = "Test book",
                        Genre = "Test genre",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "Test description",
                        ImageName = "testimage.jpeg",
                        Image = new MemoryStream([1,2,1,2,1])
                    },
                    nameof(BookForUpdateDto.ISBN)
                ],
                [
                    new BookForUpdateDto
                    {
                        ISBN = "ISBN 978059652068",
                        Title = "Test book",
                        Genre = "Test genre",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "Test description",
                        ImageName = "testimage.jpeg",
                        Image = new MemoryStream([1,2,1,2,1])
                    },
                    nameof(BookForUpdateDto.ISBN)
                ],
                [
                    new BookForUpdateDto
                    {
                        ISBN = "ISBN",
                        Title = "Test book",
                        Genre = "Test genre",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "Test description",
                        ImageName = "testimage.jpeg",
                        Image = new MemoryStream([1,2,1,2,1])
                    },
                    nameof(BookForUpdateDto.ISBN)
                ],
                [
                    new BookForUpdateDto
                    {
                        ISBN = "ISBN 97805965206861",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "A",
                        ImageName = "testimage.png",
                        Image = new MemoryStream([1,2,1,2,1])
                    },
                    nameof(BookForUpdateDto.Title)
                ],
                [
                    new BookForUpdateDto
                    {
                        ISBN = "ISBN 97805965206861",
                        Title = "AbcdefghijklmnopqrstAbcdefghijklmnopqrstu",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "A",
                        ImageName = "testimage.png",
                        Image = new MemoryStream([1,2,1,2,1])
                    },
                    nameof(BookForUpdateDto.Title)
                ],
                [
                    new BookForUpdateDto
                    {
                        ISBN = "ISBN 97805965206861",
                        Title = "A",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "A",
                        ImageName = "testimage.png",
                        Image = new MemoryStream([1,2,1,2,1])
                    },
                    nameof(BookForUpdateDto.Genre)
                ],
                [
                    new BookForUpdateDto
                    {
                        ISBN = "ISBN 97805965206861",
                        Title = "A",
                        Genre = "AbcdefghijklmnopqrstAbcdefghijk",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "A",
                        ImageName = "testimage.png",
                        Image = new MemoryStream([1,2,1,2,1])
                    },
                    nameof(BookForUpdateDto.Genre)
                ],
                [
                    new BookForUpdateDto
                    {
                        ISBN = "ISBN 97805965206861",
                        Title = "A",
                        Genre = "B",
                        Description = "A",
                        ImageName = "testimage.png",
                        Image = new MemoryStream([1,2,1,2,1])
                    },
                    nameof(BookForUpdateDto.AuthorId)
                ],
                [
                    new BookForUpdateDto
                    {
                        ISBN = "ISBN 97805965206861",
                        Title = "A",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        ImageName = "testimage.png",
                        Image = new MemoryStream([1,2,1,2,1])
                    },
                    nameof(BookForUpdateDto.Description)
                ],
                [
                    new BookForUpdateDto
                    {
                        ISBN = "ISBN 97805965206861",
                        Title = "A",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "AbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAb",
                        ImageName = "testimage.png",
                        Image = new MemoryStream([1,2,1,2,1])
                    },
                    nameof(BookForUpdateDto.Description)
                ],
                [
                    new BookForUpdateDto
                    {
                        ISBN = "ISBN 97805965206861",
                        Title = "A",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "C",
                        Image = new MemoryStream([1,2,1,2,1])
                    },
                    nameof(BookForUpdateDto.ImageName)
                ],
            ];
        }

        #endregion
    }
}
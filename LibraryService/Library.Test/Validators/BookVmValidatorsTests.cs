using FluentValidation.TestHelper;
using Library.Controllers.Common.Validators.Books;
using Library.Controllers.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Library.Test.Validators
{
    public class BookVmValidatorsTests
    {
        [Theory]
        [MemberData(nameof(GetValidBooksForCreation))]
        public void ValidBookForCreation_ShouldNotHaveValidationError(BookForCreationViewModel book)
        {
            var creationValidator = new BookForCreationVmValidator();

            var result = creationValidator.TestValidate(book);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [MemberData(nameof(GetInvalidBooksForCreation))]
        public void InvalidBookForCreation_ShouldHaveValidationError(BookForCreationViewModel book,
            string errorField)
        {
            var creationValidator = new BookForCreationVmValidator();

            var result = creationValidator.TestValidate(book);

            result.ShouldHaveValidationErrorFor(errorField);
        }

        [Theory]
        [MemberData(nameof(GetValidBooksForUpdate))]
        public void ValidBookForUpdate_ShouldNotHaveValidationError(BookForUpdateViewModel book)
        {
            var creationValidator = new BookForUpdateVmValidator();

            var result = creationValidator.TestValidate(book);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [MemberData(nameof(GetInvalidBooksForUpdate))]
        public void InvalidBookForUpdate_ShouldHaveValidationError(BookForUpdateViewModel book,
            string errorField)
        {
            var creationValidator = new BookForUpdateVmValidator();

            var result = creationValidator.TestValidate(book);

            result.ShouldHaveValidationErrorFor(errorField);
        }

        #region TestData

        public static IEnumerable<object[]> GetValidBooksForCreation()
        {
            return
            [
                [
                    new BookForCreationViewModel
                    {
                        ISBN = "ISBN 9780596520686",
                        Title = "Test book",
                        Genre = "Test genre",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "Test description",
                    }
                ],
                [
                    new BookForCreationViewModel
                    {
                        ISBN = "9780596520686",
                        Title = "A",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "A",
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "image.png")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "image/png"
                        }
                    }
                ],
                [
                    new BookForCreationViewModel
                    {
                        ISBN = "ISBN-10 0545010225",
                        Title = "A",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "A",
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "testimage.jpeg")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "image/jpeg"
                        }
                    }
                ]
            ];
        }
        
        public static IEnumerable<object[]> GetInvalidBooksForCreation()
        {
            return
            [
                [
                    new BookForCreationViewModel
                    {
                        ISBN = "ISBN 97805965206861",
                        Title = "Test book",
                        Genre = "Test genre",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "Test description",
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "image.png")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "image/png"
                        }
                    },
                    nameof(BookForCreationViewModel.ISBN)
                ],
                [
                    new BookForCreationViewModel
                    {
                        ISBN = "ISBN 978059652068",
                        Title = "Test book",
                        Genre = "Test genre",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "Test description",
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "image.png")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "image/png"
                        }
                    },
                    nameof(BookForCreationViewModel.ISBN)
                ],
                [
                    new BookForCreationViewModel
                    {
                        ISBN = "ISBN",
                        Title = "Test book",
                        Genre = "Test genre",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "Test description",
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "image.png")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "image/png"
                        }
                    },
                    nameof(BookForCreationViewModel.ISBN)
                ],
                [
                    new BookForCreationViewModel
                    {
                        ISBN = "ISBN 9780596520686",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "A",
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "image.png")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "image/png"
                        }
                    },
                    nameof(BookForCreationViewModel.Title)
                ],
                [
                    new BookForCreationViewModel
                    {
                        ISBN = "ISBN 9780596520686",
                        Title = "AbcdefghijklmnopqrstAbcdefghijklmnopqrstu",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "A",
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "image.png")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "image/png"
                        }
                    },
                    nameof(BookForCreationViewModel.Title)
                ],
                [
                    new BookForCreationViewModel
                    {
                        ISBN = "ISBN 9780596520686",
                        Title = "A",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "A",
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "image.png")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "image/png"
                        }
                    },
                    nameof(BookForCreationViewModel.Genre)
                ],
                [
                    new BookForCreationViewModel
                    {
                        ISBN = "ISBN 9780596520686",
                        Title = "A",
                        Genre = "AbcdefghijklmnopqrstAbcdefghijk",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "A",
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "image.png")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "image/png"
                        }
                    },
                    nameof(BookForCreationViewModel.Genre)
                ],
                [
                    new BookForCreationViewModel
                    {
                        ISBN = "ISBN 9780596520686",
                        Title = "A",
                        Genre = "B",
                        Description = "A",
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "image.png")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "image/png"
                        }
                    },
                    nameof(BookForCreationViewModel.AuthorId)
                ],
                [
                    new BookForCreationViewModel
                    {
                        ISBN = "ISBN 9780596520686",
                        Title = "A",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "image.png")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "image/png"
                        }
                    },
                    nameof(BookForCreationViewModel.Description)
                ],
                [
                    new BookForCreationViewModel
                    {
                        ISBN = "ISBN 9780596520686",
                        Title = "A",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "AbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAb",
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "image.png")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "image/png"
                        }
                    },
                    nameof(BookForCreationViewModel.Description)
                ],
                [
                    new BookForCreationViewModel
                    {
                        ISBN = "ISBN 9780596520686",
                        Title = "A",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "C",
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "image.png")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = " application/octet-stream "
                        }
                    },
                    nameof(BookForCreationViewModel.Image)
                ],
            ];
        }

        public static IEnumerable<object[]> GetValidBooksForUpdate()
        {
            return
            [
                 [
                    new BookForUpdateViewModel
                    {
                        ISBN = "ISBN 9780596520686",
                        Title = "Test book",
                        Genre = "Test genre",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "Test description",
                    }
                ],
                [
                    new BookForUpdateViewModel
                    {
                        ISBN = "9780596520686",
                        Title = "A",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "A",
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "image.png")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "image/png"
                        }
                    }
                ],
                [
                    new BookForUpdateViewModel
                    {
                        ISBN = "ISBN-10 0545010225",
                        Title = "A",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "A",
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "testimage.jpeg")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "image/jpeg"
                        }
                    }
                ]
            ];
        }

        public static IEnumerable<object[]> GetInvalidBooksForUpdate()
        {
            return
            [
                [
                    new BookForUpdateViewModel
                    {
                        ISBN = "ISBN 97805965206861",
                        Title = "Test book",
                        Genre = "Test genre",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "Test description",
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "testimage.jpeg")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "image/jpeg"
                        }
                    },
                    nameof(BookForUpdateViewModel.ISBN)
                ],
                [
                    new BookForUpdateViewModel
                    {
                        ISBN = "ISBN 978059652068",
                        Title = "Test book",
                        Genre = "Test genre",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "Test description",
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "testimage.jpeg")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "image/jpeg"
                        }
                    },
                    nameof(BookForUpdateViewModel.ISBN)
                ],
                [
                    new BookForUpdateViewModel
                    {
                        ISBN = "ISBN",
                        Title = "Test book",
                        Genre = "Test genre",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "Test description",
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "testimage.jpeg")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "image/jpeg"
                        }
                    },
                    nameof(BookForUpdateViewModel.ISBN)
                ],
                [
                    new BookForUpdateViewModel
                    {
                        ISBN = "ISBN 97805965206861",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "A",
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "testimage.jpeg")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "image/jpeg"
                        }
                    },
                    nameof(BookForUpdateViewModel.Title)
                ],
                [
                    new BookForUpdateViewModel
                    {
                        ISBN = "ISBN 97805965206861",
                        Title = "AbcdefghijklmnopqrstAbcdefghijklmnopqrstu",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "A",
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "testimage.jpeg")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "image/jpeg"
                        }
                    },
                    nameof(BookForUpdateViewModel.Title)
                ],
                [
                    new BookForUpdateViewModel
                    {
                        ISBN = "ISBN 97805965206861",
                        Title = "A",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "A",
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "testimage.jpeg")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "image/jpeg"
                        }
                    },
                    nameof(BookForUpdateViewModel.Genre)
                ],
                [
                    new BookForUpdateViewModel
                    {
                        ISBN = "ISBN 97805965206861",
                        Title = "A",
                        Genre = "AbcdefghijklmnopqrstAbcdefghijk",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "A",
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "testimage.jpeg")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "image/jpeg"
                        }
                    },
                    nameof(BookForUpdateViewModel.Genre)
                ],
                [
                    new BookForUpdateViewModel
                    {
                        ISBN = "ISBN 97805965206861",
                        Title = "A",
                        Genre = "B",
                        Description = "A",
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "testimage.jpeg")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "image/jpeg"
                        }
                    },
                    nameof(BookForUpdateViewModel.AuthorId)
                ],
                [
                    new BookForUpdateViewModel
                    {
                        ISBN = "ISBN 97805965206861",
                        Title = "A",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "testimage.jpeg")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "image/jpeg"
                        }
                    },
                    nameof(BookForUpdateViewModel.Description)
                ],
                [
                    new BookForUpdateViewModel
                    {
                        ISBN = "ISBN 97805965206861",
                        Title = "A",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "AbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAbcdefghijkAbcdefghijklmnopqrstAb",
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "testimage.jpeg")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "image/jpeg"
                        }
                    },
                    nameof(BookForUpdateViewModel.Description)
                ],
                [
                    new BookForUpdateViewModel
                    {
                        ISBN = "ISBN 97805965206861",
                        Title = "A",
                        Genre = "B",
                        AuthorId = new Guid("ac31fda2-411c-4669-8e42-b4b18cc659cb"),
                        Description = "C",
                        Image = new FormFile(new MemoryStream(), 0, 0, "", "image.png")
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = " application/octet-stream "
                        }
                    },
                    nameof(BookForCreationViewModel.Image)
                ],
            ];
        }

        #endregion

    }
}
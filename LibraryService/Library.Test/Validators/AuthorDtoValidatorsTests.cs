using FluentValidation.TestHelper;
using Library.UseCases.Authors.DTOs;
using Library.UseCases.Common.Validators.Authors;

namespace Library.Test.Validators
{
    public class AuthorDtoValidatorsTests
    {
        [Theory]
        [MemberData(nameof(GetValidAuthorsForCreation))]
        public void ValidAuthorForCreation_ShouldNotHaveValidationError(AuthorForCreationDto author)
        {
            var creationValidator = new AuthorForCreationDtoValidator();

            var result = creationValidator.TestValidate(author);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [MemberData(nameof(GetInvalidAuthorsForCreation))]
        public void InvalidAuthorForCreation_ShouldHaveValidationError(AuthorForCreationDto author,
            string errorField)
        {
            var creationValidator = new AuthorForCreationDtoValidator();

            var result = creationValidator.TestValidate(author);

            result.ShouldHaveValidationErrorFor(errorField);
        }

        [Theory]
        [MemberData(nameof(GetValidAuthorsForUpdate))]
        public void ValidAuthorForUpdate_ShouldNotHaveValidationError(AuthorForUpdateDto author)
        {
            var creationValidator = new AuthorForUpdateDtoValidator();

            var result = creationValidator.TestValidate(author);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [MemberData(nameof(GetInvalidAuthorsForUpdate))]
        public void InvalidAuthorForUpdate_ShouldHaveValidationError(AuthorForUpdateDto author,
            string errorField)
        {
            var creationValidator = new AuthorForUpdateDtoValidator();

            var result = creationValidator.TestValidate(author);

            result.ShouldHaveValidationErrorFor(errorField);
        }

        #region TestData
        public static IEnumerable<object[]> GetValidAuthorsForCreation()
        {
            return
            [
                [
                    new AuthorForCreationDto
                    {
                        FirstName = "Test firstname",
                        LastName = "Test lastname",
                        DateOfBirth = new DateOnly(1900, 01, 01),
                        Country = "Belarus"
                    }
                ],
                [
                    new AuthorForCreationDto
                    {
                        FirstName = "A",
                        LastName = "B",
                        DateOfBirth = new DateOnly(1000, 12, 25),
                        Country = "UK"
                    }
                ],
                [
                    new AuthorForCreationDto
                    {
                        FirstName = "Abcdefghijklmnopqrst",
                        LastName = "Abcdefghijklmnopqrst",
                        DateOfBirth = new DateOnly(2023, 12, 25),
                        Country = "Hong Kong Special Administrative Region of the People's Republic of China"
                    }
                ]
            ];
        }

        public static IEnumerable<object[]> GetInvalidAuthorsForCreation()
        {
            return
            [
                [
                    new AuthorForCreationDto
                    {
                        LastName = "Test lastname",
                        DateOfBirth = new DateOnly(1900, 01, 01),
                        Country = "Belarus"
                    },
                    nameof(AuthorForCreationDto.FirstName)
                ],
                [
                    new AuthorForCreationDto
                    {
                        FirstName = "Abcdefghijklmnopqrstq",
                        LastName = "Test lastname",
                        DateOfBirth = new DateOnly(1900, 01, 01),
                        Country = "Belarus"
                    },
                    nameof(AuthorForCreationDto.FirstName)
                ],
                [
                    new AuthorForCreationDto
                    {
                        FirstName = "Test firstname",
                        DateOfBirth = new DateOnly(1900, 01, 01),
                        Country = "Belarus"
                    },
                    nameof(AuthorForCreationDto.LastName)
                ],
                [
                    new AuthorForCreationDto
                    {
                        FirstName = "Test firstname",
                        LastName = "Abcdefghijklmnopqrstq",
                        DateOfBirth = new DateOnly(1900, 01, 01),
                        Country = "Belarus"
                    },
                    nameof(AuthorForCreationDto.LastName)
                ],
                [
                    new AuthorForCreationDto
                    {
                        FirstName = "Test firstname",
                        LastName = "Test lastname",
                        Country = "Belarus"
                    },
                    nameof(AuthorForCreationDto.DateOfBirth)
                ],
                [
                    new AuthorForCreationDto
                    {
                        FirstName = "Test firstname",
                        LastName = "Test lastname",
                        DateOfBirth = new DateOnly(2145, 01, 01),
                        Country = "Belarus"
                    },
                    nameof(AuthorForCreationDto.DateOfBirth)
                ],
                [
                    new AuthorForCreationDto
                    {
                        FirstName = "Test firstname",
                        LastName = "Test lastname",
                        DateOfBirth = new DateOnly(1900, 01, 01),
                    },
                    nameof(AuthorForCreationDto.Country)
                ],
                [
                    new AuthorForCreationDto
                    {
                        FirstName = "Test firstname",
                        LastName = "Test lastname",
                        DateOfBirth = new DateOnly(1900, 01, 01),
                        Country = "A"
                    },
                    nameof(AuthorForCreationDto.Country)
                ],
                [
                    new AuthorForCreationDto
                    {
                        FirstName = "Test firstname",
                        LastName = "Test lastname",
                        DateOfBirth = new DateOnly(1900, 01, 01),
                        Country = "Hong Kong Special Administrative Region of the People's Republic of Chinaa"
                    },
                    nameof(AuthorForCreationDto.Country)
                ],
            ];
        }

        public static IEnumerable<object[]> GetValidAuthorsForUpdate()
        {
            return
            [
                [
                    new AuthorForUpdateDto
                    {
                        FirstName = "Test firstname",
                        LastName = "Test lastname",
                        DateOfBirth = new DateOnly(1900, 01, 01),
                        Country = "Belarus"
                    }
                ],
                [
                    new AuthorForUpdateDto
                    {
                        FirstName = "A",
                        LastName = "B",
                        DateOfBirth = new DateOnly(1000, 12, 25),
                        Country = "UK"
                    }
                ],
                [
                    new AuthorForUpdateDto
                    {
                        FirstName = "Abcdefghijklmnopqrst",
                        LastName = "Abcdefghijklmnopqrst",
                        DateOfBirth = new DateOnly(2023, 12, 25),
                        Country = "Hong Kong Special Administrative Region of the People's Republic of China"
                    }
                ]
            ];
        }

        public static IEnumerable<object[]> GetInvalidAuthorsForUpdate()
        {
            return
            [
                [
                    new AuthorForUpdateDto
                    {
                        LastName = "Test lastname",
                        DateOfBirth = new DateOnly(1900, 01, 01),
                        Country = "Belarus"
                    },
                    nameof(AuthorForUpdateDto.FirstName)
                ],
                [
                    new AuthorForUpdateDto
                    {
                        FirstName = "Abcdefghijklmnopqrstq",
                        LastName = "Test lastname",
                        DateOfBirth = new DateOnly(1900, 01, 01),
                        Country = "Belarus"
                    },
                    nameof(AuthorForUpdateDto.FirstName)
                ],
                [
                    new AuthorForUpdateDto
                    {
                        FirstName = "Test firstname",
                        DateOfBirth = new DateOnly(1900, 01, 01),
                        Country = "Belarus"
                    },
                    nameof(AuthorForUpdateDto.LastName)
                ],
                [
                    new AuthorForUpdateDto
                    {
                        FirstName = "Test firstname",
                        LastName = "Abcdefghijklmnopqrstq",
                        DateOfBirth = new DateOnly(1900, 01, 01),
                        Country = "Belarus"
                    },
                    nameof(AuthorForUpdateDto.LastName)
                ],
                [
                    new AuthorForUpdateDto
                    {
                        FirstName = "Test firstname",
                        LastName = "Test lastname",
                        Country = "Belarus"
                    },
                    nameof(AuthorForUpdateDto.DateOfBirth)
                ],
                [
                    new AuthorForUpdateDto
                    {
                        FirstName = "Test firstname",
                        LastName = "Test lastname",
                        DateOfBirth = new DateOnly(2145, 01, 01),
                        Country = "Belarus"
                    },
                    nameof(AuthorForUpdateDto.DateOfBirth)
                ],
                [
                    new AuthorForUpdateDto
                    {
                        FirstName = "Test firstname",
                        LastName = "Test lastname",
                        DateOfBirth = new DateOnly(1900, 01, 01),
                    },
                    nameof(AuthorForUpdateDto.Country)
                ],
                [
                    new AuthorForUpdateDto
                    {
                        FirstName = "Test firstname",
                        LastName = "Test lastname",
                        DateOfBirth = new DateOnly(1900, 01, 01),
                        Country = "A"
                    },
                    nameof(AuthorForUpdateDto.Country)
                ],
                [
                    new AuthorForUpdateDto
                    {
                        FirstName = "Test firstname",
                        LastName = "Test lastname",
                        DateOfBirth = new DateOnly(1900, 01, 01),
                        Country = "Hong Kong Special Administrative Region of the People's Republic of Chinaa"
                    },
                    nameof(AuthorForUpdateDto.Country)
                ],
            ];
        }

        #endregion
    }
}
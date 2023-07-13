using System;
using System.Numerics;
using Bogus;
using Microsoft.CodeAnalysis.Elfie.Model.Map;
using Microsoft.EntityFrameworkCore.Migrations;
using RazorWebTongHop.Models;

#nullable disable

namespace RazorWebTongHop.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Content = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                });

            InitData(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");
        }

        private void InitData(MigrationBuilder migrationBuilder)
        {
            // Sử dụng Bogus để fake data
            Randomizer.Seed = new Random(8675309);

            var fakerArticle = new Faker<Article>();
            fakerArticle.RuleFor(a => a.Title, faker =>
            {
                // Tham số thứ 2 là 1 delegate Func với tham số là 1 đối tượng kiểu Faker
                // và giá trị trả về là 1 đối tượng có kiểu bất kì

                // Tham số thứ 2 còn có thể là 
                // Tạo ra các câu văn có 5 -> (5+5) words
                return faker.Lorem.Sentence(5, 5);
            });

            fakerArticle.RuleFor(a => a.CreateAt, faker =>
            {
                // Trả về đối tượng DateTime
                return faker.Date.Between(new DateTime(2021, 1, 1), new DateTime(2023, 7, 13));
            });

            fakerArticle.RuleFor(a => a.Content, faker =>
            {
                return faker.Lorem.Paragraphs(1, 4);
            });

            for (int i = 1; i <= 150; i++)
            {
                Article article = fakerArticle.Generate();

                // Chèn sẵn 1 số bản ghi
                migrationBuilder.InsertData(
                    table: "Articles",
                    columns: new[] { "Title", "CreateAt", "Content" },
                    values: new object[]{
                    article.Title,
                    article.CreateAt,
                    article.Content
                    }
                );
            }
        }
    }
}

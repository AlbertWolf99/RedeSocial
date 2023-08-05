using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RedeSocial.Migrations
{
    /// <inheritdoc />
    public partial class InsertSample : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var aliceSalt = DataBase.User.GenerateSalt();
            var alicePassword = DataBase.User.HashPassword(aliceSalt, "Alice##123");
            migrationBuilder.InsertData("Users", 
            new string[]{"UserName","Password","Email","BirthDay"}, 
            new object[]{"alice",alicePassword,"alice@domain.com",new DateTime(2001,01,01)});

            var bobSalt = DataBase.User.GenerateSalt();
            var bobPassword = DataBase.User.HashPassword(aliceSalt, "Bob##321");
            migrationBuilder.InsertData("Users", 
            new string[]{"UserName","Password","Email","BirthDay"}, 
            new object[]{"bob.1998",bobPassword,"bob@domain.com",new DateTime(1998,05,25)});

            migrationBuilder.InsertData("Posts",
            new string[]{"UserUserName","Text","Topic","PublishTime"},
            new object[]{"alice","texto sobre futebol","esportes",new DateTime(2023,06,25)});

            migrationBuilder.InsertData("Posts",
            new string[]{"UserUserName","Text","Topic","PublishTime"},
            new object[]{"alice","texto sobre basquete","esportes",new DateTime(2023,06,29)});

            migrationBuilder.InsertData("Posts",
            new string[]{"UserUserName","Text","Topic","PublishTime"},
            new object[]{"alice","texto sobre rugby","esportes",new DateTime(2023,07,03)});

            migrationBuilder.InsertData("Posts",
            new string[]{"UserUserName","Text","Topic","PublishTime"},
            new object[]{"alice","texto sobre pop","musica",new DateTime(2023,06,21)});
            
            migrationBuilder.InsertData("Posts",
            new string[]{"UserUserName","Text","Topic","PublishTime"},
            new object[]{"alice","texto sobre desenvolvimento web","tecnologia",new DateTime(2023,07,04)});
            
            migrationBuilder.InsertData("Posts",
            new string[]{"UserUserName","Text","Topic","PublishTime"},
            new object[]{"alice","texto sobre novo celular","tecnologia",new DateTime(2023,07,15)});

            
            migrationBuilder.InsertData("Posts",
            new string[]{"UserUserName","Text","Topic","PublishTime"},
            new object[]{"alice","texto sobre a lingua inglesa","outros",new DateTime(2023,06,28)});
            
            migrationBuilder.InsertData("Posts",
            new string[]{"UserUserName","Text","Topic","PublishTime"},
            new object[]{"bob.1998","texto sobre musica classica","musica",new DateTime(2023,06,18)});
            
            migrationBuilder.InsertData("Posts",
            new string[]{"UserUserName","Text","Topic","PublishTime"},
            new object[]{"bob.1998","texto sobre piano","musica",new DateTime(2023,07,17)});
            
            migrationBuilder.InsertData("Posts",
            new string[]{"UserUserName","Text","Topic","PublishTime"},
            new object[]{"bob.1998","texto sobre ciencia de dados","tecnologia",new DateTime(2023,07,07)});
            
            migrationBuilder.InsertData("Posts",
            new string[]{"UserUserName","Text","Topic","PublishTime"},
            new object[]{"bob.1998","texto sobre video games","tecnologia",new DateTime(2023,07,20)});
            
            migrationBuilder.InsertData("Posts",
            new string[]{"UserUserName","Text","Topic","PublishTime"},
            new object[]{"bob.1998","texto sobre receitas saudaveis","comida",new DateTime(2023,06,15)});
            
            migrationBuilder.InsertData("Posts",
            new string[]{"UserUserName","Text","Topic","PublishTime"},
            new object[]{"bob.1998","texto sobre vegetais","comida",new DateTime(2023,06,15)});
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData("Users", "UserName", "alice");
            migrationBuilder.DeleteData("Users", "UserName", "bob.1998");
        }
    }
}

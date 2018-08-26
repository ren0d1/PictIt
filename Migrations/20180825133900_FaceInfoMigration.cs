using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PictIt.Migrations
{
    public partial class FaceInfoMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Anger",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "Confidence",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "Contempt",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "Disgust",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "Fear",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "Happiness",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "Neutral",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "Sadness",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "Surprise",
                table: "Pictures");

            migrationBuilder.AddColumn<Guid>(
                name: "FaceId",
                table: "Pictures",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Blur",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BlurLevel = table.Column<string>(nullable: true),
                    Value = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blur", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Coords",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    X = table.Column<decimal>(nullable: false),
                    Y = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Emotion",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Anger = table.Column<decimal>(nullable: false),
                    Contempt = table.Column<decimal>(nullable: false),
                    Disgust = table.Column<decimal>(nullable: false),
                    Fear = table.Column<decimal>(nullable: false),
                    Happiness = table.Column<decimal>(nullable: false),
                    Neutral = table.Column<decimal>(nullable: false),
                    Sadness = table.Column<decimal>(nullable: false),
                    Surprise = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emotion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exposure",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExposureLevel = table.Column<string>(nullable: true),
                    Value = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exposure", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FaceRectangle",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Top = table.Column<int>(nullable: false),
                    Left = table.Column<int>(nullable: false),
                    Width = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaceRectangle", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FacialHair",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Moustache = table.Column<decimal>(nullable: false),
                    Beard = table.Column<decimal>(nullable: false),
                    Sideburns = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacialHair", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hair",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Bald = table.Column<decimal>(nullable: false),
                    Invisible = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hair", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Makeup",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EyeMakeup = table.Column<bool>(nullable: false),
                    LipMakeup = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Makeup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Noise",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NoiseLevel = table.Column<string>(nullable: true),
                    Value = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Noise", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Occlusion",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ForeheadOccluded = table.Column<bool>(nullable: false),
                    EyeOccluded = table.Column<bool>(nullable: false),
                    MouthOccluded = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Occlusion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FaceLandmarks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PupilLeftId = table.Column<Guid>(nullable: true),
                    PupilRightId = table.Column<Guid>(nullable: true),
                    NoseTipId = table.Column<Guid>(nullable: true),
                    MouthLeftId = table.Column<Guid>(nullable: true),
                    MouthRightId = table.Column<Guid>(nullable: true),
                    EyebrowLeftOuterId = table.Column<Guid>(nullable: true),
                    EyebrowLeftInnerId = table.Column<Guid>(nullable: true),
                    EyeLeftOuterId = table.Column<Guid>(nullable: true),
                    EyeLeftTopId = table.Column<Guid>(nullable: true),
                    EyeLeftBottomId = table.Column<Guid>(nullable: true),
                    EyeLeftInnerId = table.Column<Guid>(nullable: true),
                    EyebrowRightOuterId = table.Column<Guid>(nullable: true),
                    EyebrowRightInnerId = table.Column<Guid>(nullable: true),
                    EyeRightOuterId = table.Column<Guid>(nullable: true),
                    EyeRightTopId = table.Column<Guid>(nullable: true),
                    EyeRightBottomId = table.Column<Guid>(nullable: true),
                    EyeRightInnerId = table.Column<Guid>(nullable: true),
                    NoseRootLeftId = table.Column<Guid>(nullable: true),
                    NoseRootRightId = table.Column<Guid>(nullable: true),
                    NoseLeftAlarTopId = table.Column<Guid>(nullable: true),
                    NoseRightAlarTopId = table.Column<Guid>(nullable: true),
                    NoseLeftAlarOutTipId = table.Column<Guid>(nullable: true),
                    NoseRightAlarOutTipId = table.Column<Guid>(nullable: true),
                    UpperLipTopId = table.Column<Guid>(nullable: true),
                    UpperLipBottomId = table.Column<Guid>(nullable: true),
                    UnderLipTopId = table.Column<Guid>(nullable: true),
                    UnderLipBottomId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaceLandmarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_EyeLeftBottomId",
                        column: x => x.EyeLeftBottomId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_EyeLeftInnerId",
                        column: x => x.EyeLeftInnerId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_EyeLeftOuterId",
                        column: x => x.EyeLeftOuterId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_EyeLeftTopId",
                        column: x => x.EyeLeftTopId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_EyeRightBottomId",
                        column: x => x.EyeRightBottomId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_EyeRightInnerId",
                        column: x => x.EyeRightInnerId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_EyeRightOuterId",
                        column: x => x.EyeRightOuterId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_EyeRightTopId",
                        column: x => x.EyeRightTopId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_EyebrowLeftInnerId",
                        column: x => x.EyebrowLeftInnerId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_EyebrowLeftOuterId",
                        column: x => x.EyebrowLeftOuterId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_EyebrowRightInnerId",
                        column: x => x.EyebrowRightInnerId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_EyebrowRightOuterId",
                        column: x => x.EyebrowRightOuterId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_MouthLeftId",
                        column: x => x.MouthLeftId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_MouthRightId",
                        column: x => x.MouthRightId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_NoseLeftAlarOutTipId",
                        column: x => x.NoseLeftAlarOutTipId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_NoseLeftAlarTopId",
                        column: x => x.NoseLeftAlarTopId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_NoseRightAlarOutTipId",
                        column: x => x.NoseRightAlarOutTipId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_NoseRightAlarTopId",
                        column: x => x.NoseRightAlarTopId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_NoseRootLeftId",
                        column: x => x.NoseRootLeftId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_NoseRootRightId",
                        column: x => x.NoseRootRightId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_NoseTipId",
                        column: x => x.NoseTipId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_PupilLeftId",
                        column: x => x.PupilLeftId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_PupilRightId",
                        column: x => x.PupilRightId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_UnderLipBottomId",
                        column: x => x.UnderLipBottomId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_UnderLipTopId",
                        column: x => x.UnderLipTopId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_UpperLipBottomId",
                        column: x => x.UpperLipBottomId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceLandmarks_Coords_UpperLipTopId",
                        column: x => x.UpperLipTopId,
                        principalTable: "Coords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HairColor",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Color = table.Column<string>(nullable: true),
                    Confidence = table.Column<decimal>(nullable: false),
                    HairId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HairColor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HairColor_Hair_HairId",
                        column: x => x.HairId,
                        principalTable: "Hair",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FaceAttributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Age = table.Column<int>(nullable: false),
                    Gender = table.Column<string>(nullable: true),
                    Smile = table.Column<decimal>(nullable: false),
                    FacialHairId = table.Column<Guid>(nullable: true),
                    Glasses = table.Column<string>(nullable: true),
                    EmotionId = table.Column<Guid>(nullable: true),
                    HairId = table.Column<Guid>(nullable: true),
                    MakeupId = table.Column<Guid>(nullable: true),
                    OcclusionId = table.Column<Guid>(nullable: true),
                    BlurId = table.Column<Guid>(nullable: true),
                    ExposureId = table.Column<Guid>(nullable: true),
                    NoiseId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaceAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaceAttributes_Blur_BlurId",
                        column: x => x.BlurId,
                        principalTable: "Blur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceAttributes_Emotion_EmotionId",
                        column: x => x.EmotionId,
                        principalTable: "Emotion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceAttributes_Exposure_ExposureId",
                        column: x => x.ExposureId,
                        principalTable: "Exposure",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceAttributes_FacialHair_FacialHairId",
                        column: x => x.FacialHairId,
                        principalTable: "FacialHair",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceAttributes_Hair_HairId",
                        column: x => x.HairId,
                        principalTable: "Hair",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceAttributes_Makeup_MakeupId",
                        column: x => x.MakeupId,
                        principalTable: "Makeup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceAttributes_Noise_NoiseId",
                        column: x => x.NoiseId,
                        principalTable: "Noise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FaceAttributes_Occlusion_OcclusionId",
                        column: x => x.OcclusionId,
                        principalTable: "Occlusion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Accessory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Confidence = table.Column<decimal>(nullable: false),
                    FaceAttributesId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accessory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accessory_FaceAttributes_FaceAttributesId",
                        column: x => x.FaceAttributesId,
                        principalTable: "FaceAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Face",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FaceId = table.Column<string>(nullable: true),
                    FaceRectangleId = table.Column<Guid>(nullable: true),
                    FaceLandmarksId = table.Column<Guid>(nullable: true),
                    FaceAttributesId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Face", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Face_FaceAttributes_FaceAttributesId",
                        column: x => x.FaceAttributesId,
                        principalTable: "FaceAttributes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Face_FaceLandmarks_FaceLandmarksId",
                        column: x => x.FaceLandmarksId,
                        principalTable: "FaceLandmarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Face_FaceRectangle_FaceRectangleId",
                        column: x => x.FaceRectangleId,
                        principalTable: "FaceRectangle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_FaceId",
                table: "Pictures",
                column: "FaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Accessory_FaceAttributesId",
                table: "Accessory",
                column: "FaceAttributesId");

            migrationBuilder.CreateIndex(
                name: "IX_Face_FaceAttributesId",
                table: "Face",
                column: "FaceAttributesId");

            migrationBuilder.CreateIndex(
                name: "IX_Face_FaceLandmarksId",
                table: "Face",
                column: "FaceLandmarksId");

            migrationBuilder.CreateIndex(
                name: "IX_Face_FaceRectangleId",
                table: "Face",
                column: "FaceRectangleId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceAttributes_BlurId",
                table: "FaceAttributes",
                column: "BlurId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceAttributes_EmotionId",
                table: "FaceAttributes",
                column: "EmotionId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceAttributes_ExposureId",
                table: "FaceAttributes",
                column: "ExposureId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceAttributes_FacialHairId",
                table: "FaceAttributes",
                column: "FacialHairId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceAttributes_HairId",
                table: "FaceAttributes",
                column: "HairId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceAttributes_MakeupId",
                table: "FaceAttributes",
                column: "MakeupId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceAttributes_NoiseId",
                table: "FaceAttributes",
                column: "NoiseId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceAttributes_OcclusionId",
                table: "FaceAttributes",
                column: "OcclusionId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_EyeLeftBottomId",
                table: "FaceLandmarks",
                column: "EyeLeftBottomId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_EyeLeftInnerId",
                table: "FaceLandmarks",
                column: "EyeLeftInnerId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_EyeLeftOuterId",
                table: "FaceLandmarks",
                column: "EyeLeftOuterId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_EyeLeftTopId",
                table: "FaceLandmarks",
                column: "EyeLeftTopId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_EyeRightBottomId",
                table: "FaceLandmarks",
                column: "EyeRightBottomId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_EyeRightInnerId",
                table: "FaceLandmarks",
                column: "EyeRightInnerId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_EyeRightOuterId",
                table: "FaceLandmarks",
                column: "EyeRightOuterId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_EyeRightTopId",
                table: "FaceLandmarks",
                column: "EyeRightTopId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_EyebrowLeftInnerId",
                table: "FaceLandmarks",
                column: "EyebrowLeftInnerId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_EyebrowLeftOuterId",
                table: "FaceLandmarks",
                column: "EyebrowLeftOuterId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_EyebrowRightInnerId",
                table: "FaceLandmarks",
                column: "EyebrowRightInnerId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_EyebrowRightOuterId",
                table: "FaceLandmarks",
                column: "EyebrowRightOuterId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_MouthLeftId",
                table: "FaceLandmarks",
                column: "MouthLeftId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_MouthRightId",
                table: "FaceLandmarks",
                column: "MouthRightId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_NoseLeftAlarOutTipId",
                table: "FaceLandmarks",
                column: "NoseLeftAlarOutTipId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_NoseLeftAlarTopId",
                table: "FaceLandmarks",
                column: "NoseLeftAlarTopId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_NoseRightAlarOutTipId",
                table: "FaceLandmarks",
                column: "NoseRightAlarOutTipId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_NoseRightAlarTopId",
                table: "FaceLandmarks",
                column: "NoseRightAlarTopId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_NoseRootLeftId",
                table: "FaceLandmarks",
                column: "NoseRootLeftId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_NoseRootRightId",
                table: "FaceLandmarks",
                column: "NoseRootRightId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_NoseTipId",
                table: "FaceLandmarks",
                column: "NoseTipId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_PupilLeftId",
                table: "FaceLandmarks",
                column: "PupilLeftId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_PupilRightId",
                table: "FaceLandmarks",
                column: "PupilRightId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_UnderLipBottomId",
                table: "FaceLandmarks",
                column: "UnderLipBottomId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_UnderLipTopId",
                table: "FaceLandmarks",
                column: "UnderLipTopId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_UpperLipBottomId",
                table: "FaceLandmarks",
                column: "UpperLipBottomId");

            migrationBuilder.CreateIndex(
                name: "IX_FaceLandmarks_UpperLipTopId",
                table: "FaceLandmarks",
                column: "UpperLipTopId");

            migrationBuilder.CreateIndex(
                name: "IX_HairColor_HairId",
                table: "HairColor",
                column: "HairId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Face_FaceId",
                table: "Pictures",
                column: "FaceId",
                principalTable: "Face",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Face_FaceId",
                table: "Pictures");

            migrationBuilder.DropTable(
                name: "Accessory");

            migrationBuilder.DropTable(
                name: "Face");

            migrationBuilder.DropTable(
                name: "HairColor");

            migrationBuilder.DropTable(
                name: "FaceAttributes");

            migrationBuilder.DropTable(
                name: "FaceLandmarks");

            migrationBuilder.DropTable(
                name: "FaceRectangle");

            migrationBuilder.DropTable(
                name: "Blur");

            migrationBuilder.DropTable(
                name: "Emotion");

            migrationBuilder.DropTable(
                name: "Exposure");

            migrationBuilder.DropTable(
                name: "FacialHair");

            migrationBuilder.DropTable(
                name: "Hair");

            migrationBuilder.DropTable(
                name: "Makeup");

            migrationBuilder.DropTable(
                name: "Noise");

            migrationBuilder.DropTable(
                name: "Occlusion");

            migrationBuilder.DropTable(
                name: "Coords");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_FaceId",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "FaceId",
                table: "Pictures");

            migrationBuilder.AddColumn<bool>(
                name: "Anger",
                table: "Pictures",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Confidence",
                table: "Pictures",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Contempt",
                table: "Pictures",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Disgust",
                table: "Pictures",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Fear",
                table: "Pictures",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Happiness",
                table: "Pictures",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Neutral",
                table: "Pictures",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Sadness",
                table: "Pictures",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Surprise",
                table: "Pictures",
                nullable: false,
                defaultValue: false);
        }
    }
}

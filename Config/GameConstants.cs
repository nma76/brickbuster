namespace brickbuster.Config;

public static class GameConstants
{
    // Show debug information and objects
    public const bool Debug = true;

    // Game play parameters
    public const int PlayerLifes = 2;
    public const float IncreseBallSpeedFactor = 1.05f;
    public const float IncreseBallSpeedEndGameFactor = 1.4f;

    // Paddle parameters
    public const int PaddleWidth = 80;
    public const int PaddleHeight = 16;
    public const int PaddleMaxWidth = 220;
    public const int PaddleMinWidth = 40;

    // Ball parameters
    public const float BallRadius = 6f;
    public const float InitialBallSpeedX = 150f;
    public const float InitialBallSpeedY = -350f;
    public const float BallMaxSpeed = 1200f;

    // Block parameters
    public const int BlockWidth = 80;
    public const int BlockHeight = 30;
    public const int BlockSpacingX = 2;
    public const int BlockSpacingY = 2;

    // PowerUp parameters
    public const double PowerUpchance = 0.10;
    public const float PowerUpSpeed = 150f;
    public const int PowerUpWidth = 32;
    public const int PowerUpHeight = 16;

    // Sfx and music parameters
    public const float SfxVolume = 1.0f;
    public const float MusicVolume = 0.9f;

    // Scoring paramters
    public const int BaseBonus = 5000;
    public const float NormalMultiplier = 1f;
    public const float BossMultiplier = 1.4f;

    // Grid and boundary paramters
    public const int BorderThickness = 40;
    public const int GridStartX = 115;
    public const int GridStartY = 60;
}
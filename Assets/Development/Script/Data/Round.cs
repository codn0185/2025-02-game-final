using System;
using System.Collections.Generic;

public class Round
{
    public class RoundData
    {
        public int round; // 현재 라운드
        public float round_time; // 라운드 시간(초)
        public float mob_spawn_rate; // 몹 스폰 빈도 비율 (기본값 1.0, 5%씩 감소)
        public float mob_hp_rate; // 몹 체력 배수 (기본값 1.0, 10%씩 증가)
        public float mob_speed_rate; // 몹 속도 배수 (기본값 1.0, 2%씩 증가)

        public RoundData(int round, float round_time, float mob_spawn_rate, float mob_hp_rate, float mob_speed_rate)
        {
            this.round = round;
            this.round_time = round_time;
            this.mob_spawn_rate = mob_spawn_rate;
            this.mob_hp_rate = mob_hp_rate;
            this.mob_speed_rate = mob_speed_rate;
        }
    }

    // progression constants
    public const int MAX_ROUND = 15;
    public const float START_TIME = 10f; // seconds
    public const float TIME_STEP = 2f; // seconds per round

    public const float START_MOB_SPAWN_RATE = 1f;
    public const float MOB_SPAWN_DECREASE_PERCENT = 0.2f; // 20% decrease per round

    public const float START_MOB_HP_RATE = 1f;
    public const float MOB_HP_INCREASE_PERCENT = 0.3f; // 30% increase per round

    public const float START_MOB_SPEED_RATE = 1f;
    public const float MOB_SPEED_INCREASE_PERCENT = 0.1f; // 10% increase per round

    // data list: index matches round (0..30), data[0] is dummy
    public static readonly List<RoundData> data;

    static Round()
    {
        data = new List<RoundData>(MAX_ROUND);
        data.Add(new RoundData(0, 0, 0, 0, 0));
        for (int r = 1; r <= MAX_ROUND; r++)
        {
            float roundTime = START_TIME + (r - 1) * TIME_STEP;

            // spawn rate decreases by 5% each round (multiplicative)
            double spawn = START_MOB_SPAWN_RATE * Math.Pow(1.0 + MOB_SPAWN_DECREASE_PERCENT, r - 1);
            // hp rate increases by 10% each round (multiplicative)
            double hp = START_MOB_HP_RATE * Math.Pow(1.0 + MOB_HP_INCREASE_PERCENT, r - 1);
            // speed rate increases by 2% each round (multiplicative)
            double speed = START_MOB_SPEED_RATE * Math.Pow(1.0 + MOB_SPEED_INCREASE_PERCENT, r - 1);

            data.Add(new RoundData(r, roundTime, (float)spawn, (float)hp, (float)speed));
        }
    }
}



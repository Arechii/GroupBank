using fr34kyn01535.Uconomy;

namespace Arechi.GroupBank.Utils
{
    public static class UconomyUtil
    {
        public static decimal GetBalance(string playerId)
        {
            return Uconomy.Instance.Database.GetBalance(playerId);
        }

        public static void IncreaseBalance(string playerId, decimal amount)
        {
            Uconomy.Instance.Database.IncreaseBalance(playerId, amount);
        }
    }
}

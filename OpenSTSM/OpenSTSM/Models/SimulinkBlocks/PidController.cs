

namespace OpenSTSM.Models.SimulinkBlocks
{
    public class PidController
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public PidController(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public PidController(PidController controller)
        {
            this.Id = controller.Id;
            this.Name = controller.Name;
        }
    }
}

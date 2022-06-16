namespace Pokedex_Form
{
    public partial class Form1 : Form
    {
        const string URL_API = @"https://pokeapi.co/api/v2/pokemon/";
        List<string> listaImagenes = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            Pokemon objPokemon = await BuscarPokemon("1");
            PintarPokemon(objPokemon);
        }

        private void CargarImagenes(Pokemon objPokemon)
        {
            listaImagenes.Clear();
            if (objPokemon.sprites.front_default is not null) listaImagenes.Add(objPokemon.sprites.front_default);
            if (objPokemon.sprites.back_default is not null) listaImagenes.Add(objPokemon.sprites.back_default);
            if (objPokemon.sprites.back_shiny is not null) listaImagenes.Add(objPokemon.sprites.back_shiny); ;
            if (objPokemon.sprites.front_shiny is not null) listaImagenes.Add(objPokemon.sprites.front_shiny);            
            
        }

        private void PintarPokemon(Pokemon objPokemon)
        {
            CargarImagenes(objPokemon);
            lbNombre.Text = objPokemon.name.ToUpper();
            lbID.Text = "#" + objPokemon.id.ToString();
            picturePokemon.Load(listaImagenes[0]);
            picturePokemon.SizeMode = PictureBoxSizeMode.Zoom;
            lbTipo1.Text = objPokemon.types[0].type.name;
            lbTipo2.Text = objPokemon.types.Length > 1 ? objPokemon.types[1].type.name : "";
            
            lbHabilidades.ResetText();
            lbHabilidades.Text = lbHabilidades.Text += "| " + objPokemon.abilities[0].ability.name + " | ";
            lbHabilidades.Text += objPokemon.abilities.Length > 1 ? objPokemon.abilities[1].ability.name + " |" : "";

            lbPuntosVida.Text = objPokemon.stats[0].base_stat.ToString();
            lbAtaque.Text = objPokemon.stats[1].base_stat.ToString();
            lbDefensa.Text = objPokemon.stats[2].base_stat.ToString();
            lbVelocidad.Text = objPokemon.stats.Last().base_stat.ToString();

            int sumaTotal = objPokemon.stats[0].base_stat + objPokemon.stats[1].base_stat + objPokemon.stats[2].base_stat + objPokemon.stats.Last().base_stat;
            lbTotal.Text = sumaTotal.ToString();

            barPuntosVida.Value = objPokemon.stats[0].base_stat;
            barAtaque.Value = objPokemon.stats[1].base_stat;
            barDefensa.Value = objPokemon.stats[2].base_stat;
            barVelocidad.Value = objPokemon.stats.Last().base_stat;

        }

        private async Task<Pokemon> BuscarPokemon(string id_)
        {
            var cliente = new HttpClient();
            var response = await cliente.GetStringAsync(URL_API + id_);
            var objPokemon = Pokemon.FromJson(response);
            return objPokemon;
        }

        private async void btnPokemonSiguiente_Click(object sender, EventArgs e)
        {
            string idx = string.IsNullOrWhiteSpace(lbID.Text)? "1" : (int.Parse(lbID.Text.Substring(1)) + 1).ToString();
            var pokemon = await BuscarPokemon(idx);
            PintarPokemon(pokemon);
        }

        private async void btnPokemonAnterior_Click(object sender, EventArgs e)
        {
            string idx = string.IsNullOrWhiteSpace(lbID.Text) || int.Parse(lbID.Text.Substring(1)) <= 1 ? "1" : (int.Parse(lbID.Text.Substring(1)) - 1).ToString();
            var pokemon = await BuscarPokemon(idx);
            PintarPokemon(pokemon);
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            string id = txtIDPokemon.Text;
            if (!string.IsNullOrWhiteSpace(id))
            {
                var pokemon = await BuscarPokemon(id);
                PintarPokemon(pokemon);
            }
            else
            {
                MessageBox.Show("Error. Debe ingresar un valor correcto",
                    "Buscando Pokemon",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
            }
        }

        private void btnImageNext_Click(object sender, EventArgs e)
        {
            string imagen = picturePokemon.ImageLocation;
            int idx = listaImagenes.IndexOf(imagen);
            int nuevoIdx = idx != listaImagenes.Count - 1 ? idx + 1 : 0;

            picturePokemon.Load(listaImagenes[nuevoIdx]);
            picturePokemon.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void btnImagePrevius_Click(object sender, EventArgs e)
        {
            string imagen = picturePokemon.ImageLocation;
            int idx = listaImagenes.IndexOf(imagen);
            int nuevoIdx = idx != 0 ? idx - 1 : listaImagenes.Count - 1;

            picturePokemon.Load(listaImagenes[nuevoIdx]);
            picturePokemon.SizeMode = PictureBoxSizeMode.Zoom;
        }
    }
}

// This file has been generated by the GUI designer. Do not modify.
namespace EoD
{
	public partial class TempWindow
	{
		private global::Gtk.VBox vbox3;
		
		private global::Gtk.Label label3;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget EoD.TempWindow
			this.WidthRequest = 200;
			this.HeightRequest = 30;
			this.Name = "EoD.TempWindow";
			this.Title = global::Mono.Unix.Catalog.GetString ("Creating...");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			this.Resizable = false;
			this.AllowGrow = false;
			this.DestroyWithParent = true;
			this.Gravity = ((global::Gdk.Gravity)(5));
			// Container child EoD.TempWindow.Gtk.Container+ContainerChild
			this.vbox3 = new global::Gtk.VBox ();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			// Container child vbox3.Gtk.Box+BoxChild
			this.label3 = new global::Gtk.Label ();
			this.label3.Name = "label3";
			this.label3.LabelProp = global::Mono.Unix.Catalog.GetString ("label3");
			this.vbox3.Add (this.label3);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.label3]));
			w1.Position = 1;
			w1.Expand = false;
			w1.Fill = false;
			this.Add (this.vbox3);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 200;
			this.DefaultHeight = 30;
			this.Show ();
		}
	}
}

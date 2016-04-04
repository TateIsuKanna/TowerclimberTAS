Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices

Public Class Form1
	Dim WindowRect As New Rectangle(24, 161, 263 - 24 + 1, 400 - 161 + 1)
	Dim screenimg As Bitmap
	Dim g As Graphics
	Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
		screenimg = New Bitmap(WindowRect.Width, WindowRect.Height)
		g = Graphics.FromImage(screenimg)
		Timer1.Start()
	End Sub

	Dim my_position As Point
	Dim my_direction As direction

	Private Enum direction
		Left
		Right
	End Enum

	Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
		g.CopyFromScreen(WindowRect.Location, New Point(0, 0), screenimg.Size)

		Dim bmpData As BitmapData = screenimg.LockBits(New Rectangle(0, 0, screenimg.Width, screenimg.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb)
		Dim pixels As Byte() = New Byte(bmpData.Stride * bmpData.Height - 1) {}
		Marshal.Copy(bmpData.Scan0, pixels, 0, pixels.Length)

		screenimg.UnlockBits(bmpData)
		Dim done As Boolean
		For y = 0 To bmpData.Height - 1
			Dim pos As Integer = y * bmpData.Stride
			For x = 0 To bmpData.Width - 1 - 1
				If pixels(pos + 2 + 3) = 0 AndAlso pixels(pos + 1 + 3) = 0 AndAlso pixels(pos + 3) = 0 Then
					If pixels(pos + 2) = 229 AndAlso pixels(pos + 1) = 172 AndAlso pixels(pos) = 110 Then
						my_position = New Point(x, y)
						my_direction = direction.Right
						done = True
						Exit For
					End If
				End If
				If pixels(pos + 2) = 0 AndAlso pixels(pos + 1) = 0 AndAlso pixels(pos) = 0 Then
					If pixels(pos + 2 + 3) = 229 AndAlso pixels(pos + 1 + 3) = 172 AndAlso pixels(pos + 3) = 110 Then
						my_position = New Point(x, y)
						my_direction = direction.Left
						done = True
						Exit For
					End If
				End If
				pos += 3
			Next
			If done Then
				Exit For
			End If
		Next


		If my_direction = direction.Left Then
			Label1.Text = "←"
		Else
			Label1.Text = "→"
		End If
		Me.CreateGraphics.DrawImage(screenimg, 0, 0)
		Me.CreateGraphics.DrawLine(Pens.Yellow, my_position.X, 0, my_position.X, screenimg.Height)
		Me.CreateGraphics.DrawLine(Pens.Yellow, 0, my_position.Y, screenimg.Width, my_position.Y)
	End Sub
End Class
Public Class MenuPrincipal

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        With Proforma
            .ShowDialog()
            .Dispose()
        End With
    End Sub
End Class

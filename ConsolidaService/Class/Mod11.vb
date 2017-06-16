Public Class Mod11

    Public Shared Function AddCheckDigit(number As String) As String
        Dim Sum As Integer = 0
        Dim i As Integer = number.Length - 1, Multiplier As Integer = 10

        While i >= 0

            If Char.IsDigit(number(i)) Then
                Sum += CInt(Math.Truncate(Double.Parse(number(i)))) * Multiplier
            Else
                Sum += CInt(Math.Truncate(Double.Parse(New Mod11().ToNumber(number(i))))) * Multiplier
            End If


            If System.Threading.Interlocked.Increment(Multiplier) = 11 Then
                Multiplier = 2
            End If
            i -= 1
        End While

        Dim Validator As String = (Sum Mod 11).ToString()

        If Validator = "11" Then
            Validator = "0"
        ElseIf Validator = "10" Then
            Validator = "X"
        End If

        Return Validator
    End Function

    Private Function ToNumber(ByVal letra As String) As String

        If letra = "A" Then
            letra = "1"
        ElseIf letra = "B" Then
            letra = "2"
        ElseIf letra = "C" Then
            letra = "3"
        ElseIf letra = "D" Then
            letra = "4"
        ElseIf letra = "E" Then
            letra = "5"
        ElseIf letra = "F" Then
            letra = "6"
        ElseIf letra = "G" Then
            letra = "7"
        ElseIf letra = "H" Then
            letra = "8"
        ElseIf letra = "J" Then
            letra = "1"
        ElseIf letra = "K" Then
            letra = "2"
        ElseIf letra = "L" Then
            letra = "3"
        ElseIf letra = "M" Then
            letra = "4"
        ElseIf letra = "N" Then
            letra = "5"
        ElseIf letra = "P" Then
            letra = "7"
        ElseIf letra = "R" Then
            letra = "9"
        ElseIf letra = "S" Then
            letra = "2"
        ElseIf letra = "T" Then
            letra = "3"
        ElseIf letra = "U" Then
            letra = "4"
        ElseIf letra = "V" Then
            letra = "5"
        ElseIf letra = "W" Then
            letra = "6"
        ElseIf letra = "X" Then
            letra = "7"
        ElseIf letra = "Y" Then
            letra = "8"
        ElseIf letra = "Z" Then
            letra = "9"
        End If

        Return letra
            

       

    End Function

End Class

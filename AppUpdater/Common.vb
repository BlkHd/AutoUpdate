Imports System.Runtime.InteropServices

Namespace AppUpdater

    Public NotInheritable Class Common

        Public Shared Function VolatileRead(Of T)(ByRef Address As T) As T
            VolatileRead = Address
            Threading.Thread.MemoryBarrier()
        End Function

        Public Shared Sub VolatileWrite(Of T)(ByRef Address As T, ByVal Value As T)
            Threading.Thread.MemoryBarrier()
            Address = Value
        End Sub

    End Class

End Namespace

open System.Runtime.InteropServices
open System

[<Literal>]
let libPath = @"/home/paz/_p/fsharp_interop/rust_backend/target/debug/librust_backend.so"

type NativeCallback = delegate of unativeint -> unit

module InteropWithNative =
    [<DllImport(libPath, CallingConvention = CallingConvention.Cdecl)>]
    extern void hello_rust()
    [<DllImport(libPath, CallingConvention = CallingConvention.Cdecl)>]
    extern void into_callback(unativeint value, [<MarshalAs(UnmanagedType.FunctionPtr)>]NativeCallback callback)

// For more information see https://aka.ms/fsharp-console-apps
printfn "Hello from F#"


InteropWithNative.hello_rust();

let print v
    = Console.WriteLine(v.ToString())

print 33

InteropWithNative.into_callback(3un, print)
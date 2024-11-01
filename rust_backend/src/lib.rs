
#[no_mangle]
pub extern "C" fn add(left: i32, right: i32) -> i32 {
  left + right
}

#[no_mangle]
pub extern "C" fn hello_rust() {
  println!("Hello Rust");
}

#[no_mangle]
pub extern "C" fn into_callback(value: usize, callback: extern fn(usize)) {
  callback(value);
}
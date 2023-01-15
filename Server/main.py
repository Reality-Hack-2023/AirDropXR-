#!/usr/env python3
import http.server
import socketserver
import io
import cgi
import os

# Change this to serve on a different port
PORT = 44444

class CustomHTTPRequestHandler(http.server.SimpleHTTPRequestHandler):

    def do_POST(self):        
        r, info = self.deal_post_data()
        print(r, info, "by: ", self.client_address)
        f = io.BytesIO()
        if r:
            f.write(b"Success\n")
        else:
            f.write(b"Failed\n")
        length = f.tell()
        f.seek(0)
        self.send_response(200)
        self.send_header("Content-type", "text/plain")
        self.send_header("Content-Length", str(length))
        self.end_headers()
        if f:
            self.copyfile(f, self.wfile)
            f.close()      

    def deal_post_data(self):
        ctype, pdict = cgi.parse_header(self.headers['Content-Type'])
        pdict['boundary'] = bytes(pdict['boundary'], "utf-8")
        pdict['CONTENT-LENGTH'] = int(self.headers['Content-Length'])
        # print(type(self.rfile))
        if ctype == 'multipart/form-data':
            # form = cgi.FieldStorage(self.rfile)
            form = cgi.FieldStorage(fp=self.rfile, headers=self.headers, environ={'REQUEST_METHOD':'POST', 'CONTENT_TYPE':self.headers['Content-Type'], })
            # print (type(form))
            # print(form.headers)
            # print(form.value)
            try:
                if isinstance(form["file"], list):
                    for record in form["file"]:
                        open("server_download/%s"%record.filename, "wb").write(record.file.read())
                else:
                    open("server_download/%s"%form["file"].filename, "wb").write(form["file"].file.read())
            except IOError:
                    return (False, "Can't create file to write, do you have permission to write?")
        return (True, "Files uploaded")

    # def do_GET(self):
    #     dir = "server_download/"
    #     filename = "temp.jpeg"
    #     filepath = os.path.join(dir, filename)
    #     f = open(filepath, 'rb')
    #     self.send_response(200)
    #     self.send_header("Content-type", "image/jpeg")
    #     self.end_headers()
    #     while True:
    #         file_data = f.read(32768) # use an appropriate chunk size
    #         if file_data is None or len(file_data) == 0:
    #             break
    #         self.wfile.write(file_data) 
    #     f.close()
    #     try:
    #         os.remove(filepath)
    #     except OSError:
    #         print("Failed with:", OSError.strerror)
            

Handler = CustomHTTPRequestHandler
with socketserver.TCPServer(("", PORT), Handler) as httpd:
    print("serving at port", PORT)
    httpd.serve_forever()
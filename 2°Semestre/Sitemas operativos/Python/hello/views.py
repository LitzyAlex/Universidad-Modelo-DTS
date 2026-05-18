from django.http import HttpResponse

def hello_world(request):
    return HttpResponse("Aplicación de python con django  :D")

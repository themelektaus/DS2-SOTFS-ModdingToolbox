const body = document.body

const baseSize = {
    width: 960,
    height: 540
}

setInterval(() =>
{
    const width = window.innerWidth
    const height = window.innerHeight
    const zoom = Math.min(height / baseSize.height, width / baseSize.width)
    body.style.zoom = zoom
}, 15)

function scrollToBottom(element)
{
    element.scrollTo({ left: 0, top: element.scrollHeight, behavior: "smooth" })
}

function setClass(element, className, active)
{
    if (active)
    {
        element.classList.add(className)
        return
    }

    element.classList.remove(className)
}
const body = document.body

const baseSize = {
    width: body.offsetWidth,
    height: body.offsetHeight
}

setInterval(() => {
    const width = body.offsetWidth
    const height = body.offsetHeight
    const scale = Math.min(height / baseSize.height, width / baseSize.width)
    document.querySelectorAll("[scaleable]").forEach(x => x.style.scale = scale)
    document.querySelectorAll("select *").forEach(x => x.style.zoom = scale)
}, 15)